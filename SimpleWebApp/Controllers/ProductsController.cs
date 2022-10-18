using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Models;

namespace SimpleWebApp.Controllers;

public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    private readonly NorthwindContext _context;
    private int _productsToShow = 0;

    public ProductsController(
        ILogger<ProductsController> logger,
        NorthwindContext context,
        IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _productsToShow = configuration.GetValue<int>("ProductsToShow");
    }

    [HttpGet]
    public IActionResult List()
        =>View(_context.Products.Take(_productsToShow));
    
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
        return RedirectToAction("List","Products");
    }
}