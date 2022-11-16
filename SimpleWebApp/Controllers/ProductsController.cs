﻿using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Helpers;
using Microsoft.Extensions.Options;
using SimpleWebApp.Models;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;

namespace SimpleWebApp.Controllers;

public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IDbContextWrapper _dbContextWrapper;
    private readonly int _productsToShow = 0;

    public ProductsController(
        ILogger<ProductsController> logger,
        IDbContextWrapper dbContextWrapper,
        IOptions<AppOptions> opt)
    {
        _logger = logger;
        _dbContextWrapper = dbContextWrapper;
        _productsToShow = opt.Value.MaxProductsToShow;
    }

    [Route("{controller}/{action}")]
    [Route("{controller}")]
    [HttpGet]
    public IActionResult List()
    {
        var listPage = new MvcBreadcrumbNode("List", "Products", "Products") ;
        ViewData["BreadcrumbNode"] = listPage;
        ViewData["Title"] = listPage.Title;
        return View(_dbContextWrapper.GetProductsFromDb(_productsToShow));
    }

    [HttpGet]
    public IActionResult AddUpdateProduct(int productId)
    {
        var product = _dbContextWrapper.ProductToAddOrUpdate(productId).GetAwaiter().GetResult();
        
        var breadCrumbsTitle = "UpdateProduct";
        if (product.ProductId == 0)
            breadCrumbsTitle = "Creat Product";
        var listPage = new MvcBreadcrumbNode("AddUpdateProduct", "Products", breadCrumbsTitle)
        {
            Parent = new MvcBreadcrumbNode("List", "Products", "Products") 
        };
        ViewData["BreadcrumbNode"] = listPage;
        ViewData["Title"] = listPage.Title;
        
        return View(product);
    }

    [HttpPost]
    public IActionResult AddUpdateProduct(Product p)
    {
        if(_dbContextWrapper.AddOrUpdateProduct(p).GetAwaiter().GetResult())
            return RedirectToAction("List","Products");
        return RedirectToAction("AddUpdateProduct","Products", new { productId = p.ProductId });
    }
}