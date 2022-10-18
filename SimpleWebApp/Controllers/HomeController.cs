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
    
    public IActionResult Products()
        => View(_context.Products.Take(_productsToShow));
    
    public IActionResult Categories()
        => View(_context.Categories);
    

    [HttpGet]
    public IActionResult AddUpdateProduct(int productId)
    {
        var product = _context.Products.FirstOrDefault(x => x.ProductId == productId) ?? new Product();
        product.UpdateDependantProperties(_context);
        return View(product);
    }

    [HttpPost]
    public IActionResult AddUpdateProduct(Product p)
    {
        var product = _context.Products.FirstOrDefault(x => x.ProductId == p.ProductId);
        if (product == null)
        {
            if (!ModelState.IsValid)
            {
                p.UpdateDependantProperties(_context);
                return View(p);
            }
            _context.Products.AddAsync(p).GetAwaiter().GetResult();
        }
        else
        {
            if (!ModelState.IsValid)
            {
                p.UpdateDependantProperties(_context);
                return View(p);
            }
            product.UpdateProduct(p);
            _context.Products.Update(product);
        }

        _context.SaveChangesAsync().GetAwaiter().GetResult();
        return RedirectToAction("Products","Home");
    }

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