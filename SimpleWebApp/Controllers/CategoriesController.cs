﻿using System.Buffers.Text;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Helpers;
using SimpleWebApp.Models;
using SimpleWebApp.ViewModels;
using SmartBreadcrumbs.Nodes;

namespace SimpleWebApp.Controllers;

public class CategoriesController : Controller
{
    private readonly IDbContextWrapper _dbContextWrapper;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(
        IDbContextWrapper dbContextWrapper,
        ILogger<CategoriesController> logger)
    {
        _dbContextWrapper = dbContextWrapper;
        _logger = logger;
    }

    [Route("{controller}/{action}")]
    [Route("{controller}")]
    [HttpGet]
    public IActionResult List()
    {
        var listPage = new MvcBreadcrumbNode("List", "Categories", "Categories") ;
        ViewData["BreadcrumbNode"] = listPage;
        ViewData["Title"] = listPage.Title;
        
        foreach (var cat in _dbContextWrapper.GetCategoriesFromDb())
        {
            HttpContext.Items[$"imgString_{cat.CategoryId.ToString()}"] =
                _dbContextWrapper.GetCategoryFromDb(cat.CategoryId)
                    .GetAwaiter()
                    .GetResult()
                    .GetBase64Image();
        }

        return View(_dbContextWrapper.GetCategoriesFromDb());
    }
    
    [HttpGet("{controller}/{action}/{imageId}")]
    public IActionResult ImageForm(int imageId)
    {
        Category tmp = null;
        try
        {
            tmp = _dbContextWrapper.GetCategoryFromDb(imageId)
                .GetAwaiter().GetResult();
            HttpContext.Items["imgString"] = imageId;
            HttpContext.Items[$"imgString_{imageId}"] = tmp.GetBase64Image();
        }
        catch (CategoryNotFoundException e)
        {
            ModelState.AddModelError("Category", "No Category with such Id.");
            return View(imageId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Some Error during retrieving image from db");
        }

        var listPage = new MvcBreadcrumbNode("ImageForm",
            "Categories",
            $"Update image for {tmp?.CategoryName}")
        {
            Parent = new MvcBreadcrumbNode("List", "Categories", "Categories")
        } ;
        ViewData["BreadcrumbNode"] = listPage;
        ViewData["Title"] = listPage.Title;
        
        return View(imageId);
    }

    [HttpPost("{controller}/{action}/{imageId}")]
    public async Task<IActionResult> ImageForm(int imageId, IFormFile file)
    {
        if (file == null)
        {
            ModelState.AddModelError("File", "File was not found. Please try again.");
            return View(imageId);
        } 
        if (file.Length > 300000)
        {
            ModelState.AddModelError("File", "File is exceed 300 kB");
            return View(imageId);
        }
        if (imageId==null)
        {
            ModelState.AddModelError("Category", "No Category with such Id.");
            return View(imageId);
        }

        if (!_dbContextWrapper.AddUpdateCategory(new Category(imageId, file)).GetAwaiter().GetResult())
            ModelState.AddModelError("File", "Unsuccessful updating of picture");
        ;

        return RedirectToAction("ImageForm", "Categories", new { imageId });
    }

    [HttpGet("Image/{imageId}")]
    public IActionResult Image(int imageId)
    {
        Category tmp = null;
        try
        {
            tmp = _dbContextWrapper.GetCategoryFromDb(imageId)
                .GetAwaiter().GetResult();
            HttpContext.Items["imgString"] = imageId;
            HttpContext.Items[$"imgString_{imageId}"] = tmp.GetBase64Image();
        }
        catch (CategoryNotFoundException e)
        {
            _logger.LogError(e,"No Category with such Id");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Some Error during retrieving image from db");
        }
        
        var listPage = new MvcBreadcrumbNode("Image",
            "Categories",
            $"Image for {tmp?.CategoryName} category") ;
        ViewData["BreadcrumbNode"] = listPage;
        ViewData["Title"] = listPage.Title;
        
        return View(imageId);
    }
}