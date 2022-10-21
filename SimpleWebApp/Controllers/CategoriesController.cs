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
        foreach (var cat  in _dbContextWrapper.GetCategoriesFromDb())
        {
            ViewData[cat.CategoryId.ToString()] =
                    _dbContextWrapper.GetCategoryFromDb(cat.CategoryId)
                        .GetAwaiter()
                        .GetResult()
                        .GetBase64Image();
        }
        return View(_dbContextWrapper.GetCategoriesFromDb());
    }

    [Route("Image/{imageId}")]
    [Route("{controller}/{action}/{imageId}")]
    [HttpGet]
    public IActionResult Image( int imageId )
    {
        ViewData["imgString"] = _dbContextWrapper.GetCategoryFromDb(imageId)
            .GetAwaiter().
            GetResult()
            .GetBase64Image();
        return View(imageId);
    }

    [Route("Image/{imageId}")]
    [Route("{controller}/{action}/{imageId}")]
    [HttpPost]
    public async Task<IActionResult> Image(int imageId, IFormFile file)
    {
        if (file == null)
            throw new Exception("smthgowrng");
        if (file.Length > 300000)
        {
            ModelState.AddModelError("File", "File is too large");
            return View(imageId);
        }

        if (!_dbContextWrapper.AddUpdateCategory(new Category(imageId, file)).GetAwaiter().GetResult())
            ModelState.AddModelError("File", "Unsuccessful updating of picture");;
        
        return RedirectToAction("List","Categories");
    }
}