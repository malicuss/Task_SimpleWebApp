using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Models;

namespace SimpleWebApp.Controllers;

public class CategoriesController : Controller
{
    private readonly NorthwindContext _context;

    public CategoriesController(NorthwindContext context)
    {
        _context = context;
    }

    public IActionResult List()
    {
        return View(_context.Categories);
    }
}