using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Helpers;
using SimpleWebApp.Models;

namespace SimpleWebApp.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ApiController : ControllerBase
{
    private readonly ILogger<ApiController> _logger;
    private readonly IDbContextWrapper _dbContextWrapper;

    public ApiController(ILogger<ApiController> logger,IDbContextWrapper dbContextWrapper )
    {
        _logger = logger;
        _dbContextWrapper = dbContextWrapper;
    }

    [HttpGet]
    public async Task<ActionResult<Category>> GetCategory([FromQuery]int categoryId) 
        => await _dbContextWrapper.GetCategoryFromDb(categoryId);
    
    [HttpGet]
    public async Task<ActionResult<Product>> GetProduct([FromQuery]int productId) 
        => await _dbContextWrapper.GetProductFromDb(productId);
    
    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetCategories() 
        => await _dbContextWrapper.GetCategoriesFromDb();
    
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts() 
        => await _dbContextWrapper.GetAllProductsFromDb();

    [HttpPost]
    public async Task<bool> CreateProduct([FromForm]Product p) 
        => await _dbContextWrapper.AddOrUpdateProduct(p);
    
    [HttpPost]
    public async Task<bool> UpdateProduct([FromForm]Product p)
        => await _dbContextWrapper.AddOrUpdateProduct(p);
    
    [HttpDelete]
    public async Task<bool> DeleteProduct([FromForm]Product p)
        =>await _dbContextWrapper.DeleteProduct(p);

    [HttpGet]
    public async Task<string> GetCategoryImage([FromQuery]int categoryId)
    {
        var category = await _dbContextWrapper.GetCategoryFromDb(categoryId);
        return category.GetBase64Image();
    }

    [HttpPost]
    public async Task<bool> UpdateCategoryImage([FromQuery]int categoryId,[FromForm] IFormFile file)
        => await _dbContextWrapper.AddUpdateCategory(new Category(categoryId, file));
}