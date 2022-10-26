using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Helpers;
using Microsoft.Extensions.Options;
using SimpleWebApp.Models;

namespace SimpleWebApp.Controllers;

public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IDbContextWrapper _dbContextWrapper;
    private int _productsToShow = 0;

    public ProductsController(
        ILogger<ProductsController> logger,
        IDbContextWrapper dbContextWrapper,
        IConfiguration configuration,
        NorthwindContext context,
        IOptions<AppOptions> opt)
    {
        _logger = logger;
        _dbContextWrapper = dbContextWrapper;
        _productsToShow = configuration.GetValue<int>("ProductsToShow");
        _context = context;
        _productsToShow = opt.Value.MaxProductsToShow;
    }

    [HttpGet]
    public IActionResult List()
        =>View(_dbContextWrapper.GetProductsFromDb(_productsToShow));
    
    [HttpGet]
    public IActionResult AddUpdateProduct(int productId)
    {
        var product = _dbContextWrapper.GetProductFromDb(productId).GetAwaiter().GetResult();
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