using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Models;
using SimpleWebApp.ViewModels;

namespace SimpleWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly NorthwindContext _context;
    private readonly int _productsToShow;

    public HomeController(
        ILogger<HomeController> logger, 
        NorthwindContext context,
        IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _productsToShow = configuration.GetValue<int>("ProductsToShow");
    }

    public IActionResult Index()
        => View();

    public IActionResult Categories()
        => View(_context.Categories.ToList());

    public IActionResult ShowMeException()
    {
        throw new Exception("This is test exception");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}