using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleWebApp.Helpers;
using SimpleWebApp.Models;

namespace SimpleWebApp.Controllers;

public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    private readonly NorthwindContext _context;
    private readonly int _productsToShow;

    public ProductsController(
        ILogger<ProductsController> logger,
        NorthwindContext context,
        IOptions<AppOptions> opt)
    {
        _logger = logger;
        _context = context;
        _productsToShow = opt.Value.MaxProductsToShow;
    }

    [HttpGet]
    public IActionResult List()
    {
        if (_productsToShow > 0)
        {
            return View(_context.Products.Take(_productsToShow));
        }
        return View(_context.Products);
    }

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