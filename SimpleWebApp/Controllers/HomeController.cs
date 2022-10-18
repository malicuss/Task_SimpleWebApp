using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Models;
using SimpleWebApp.ViewModels;

namespace SimpleWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
        => View();

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