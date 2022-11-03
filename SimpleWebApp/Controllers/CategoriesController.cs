using System.Buffers.Text;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Helpers;
using SimpleWebApp.Models;
using SimpleWebApp.ViewModels;

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

    public IActionResult List()
    {
        foreach (var cat in _dbContextWrapper.GetCategoriesFromDb())
        {
            ViewData[cat.CategoryId.ToString()] =
                _dbContextWrapper.GetCategoryFromDb(cat.CategoryId)
                    .GetAwaiter()
                    .GetResult()
                    .GetBase64Image();
        }

        return View(_dbContextWrapper.GetCategoriesFromDb());
    }

    [Route("{controller}/{action}/{imageId}")]
    [HttpGet]
    public IActionResult ImageForm(int imageId)
    {
        try
        {
            ViewData["imgString"] = _dbContextWrapper.GetCategoryFromDb(imageId)
                .GetAwaiter().GetResult()
                .GetBase64Image();
        }
        catch (CategoryNotFoundException e)
        {
            //left for good. Do not know what to do here.
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Some Error during retrieving image from db");
        }

        
        return View(imageId);
    }

    [Route("{controller}/{action}/{imageId}")]
    [HttpPost]
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

        if (!_dbContextWrapper.AddUpdateCategory(new Category(imageId, file)).GetAwaiter().GetResult())
            ModelState.AddModelError("File", "Unsuccessful updating of picture");
        ;

        return RedirectToAction("ImageForm", "Categories", new { imageId });
    }

    [Route("Image/{imageId}")]
    public IActionResult Image(int imageId)
    {
        try
        {
            ViewData["imgString"] = _dbContextWrapper.GetCategoryFromDb(imageId)
                .GetAwaiter().GetResult()
                .GetBase64Image();
        }
        catch (CategoryNotFoundException e)
        {
            //left for good. Do not know what to do here.
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Some Error during retrieving image from db");
        }
        
        return View(imageId);
    }
}