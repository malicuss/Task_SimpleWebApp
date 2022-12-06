using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Core.Helpers;
using SimpleWebApp.Core.Models;

namespace SimpleWebApp.API.Controllers;

[ApiController]
[Route("api/[action]")]
public class ApiController : ControllerBase
{
    private readonly ILogger<ApiController> _logger;
    private readonly IDbContextWrapper _dbContextWrapper;

    public ApiController(ILogger<ApiController> logger,IDbContextWrapper dbContextWrapper )
    {
        _logger = logger;
        _dbContextWrapper = dbContextWrapper;
    }

    [HttpGet("categoryId")]
    public async Task<ActionResult<Category>> Category([FromQuery]int categoryId) 
        => await _dbContextWrapper.GetCategoryFromDb(categoryId);
    
    [HttpGet("productId")]
    public async Task<ActionResult<Product>> Product([FromQuery]int productId) 
        => await _dbContextWrapper.GetProductFromDb(productId);
    
    [HttpGet]
    public async Task<ActionResult<List<Category>>> Categories() 
        => await _dbContextWrapper.GetCategoriesFromDb();
    
    [HttpGet]
    public async Task<ActionResult<List<Product>>> Products() 
        => await _dbContextWrapper.GetAllProductsFromDb();

    [HttpPost("product")]
    public async Task<bool> PostProduct([FromForm]Product product) 
        => await _dbContextWrapper.CreateProduct(product);
    
    [HttpPut("product")]
    public async Task<bool> PutProduct([FromForm]Product product)
        => await _dbContextWrapper.UpdateProduct(product);
    
    [HttpDelete("product")]
    public async Task<bool> Product([FromForm]Product product)
        =>await _dbContextWrapper.DeleteProduct(product);

    [HttpGet("categoryId")]
    public async Task<string> CategoryImage([FromQuery]int categoryId)
    {
        var category = await _dbContextWrapper.GetCategoryFromDb(categoryId);
        return category.GetBase64Image();
    }

    [HttpPut]
    public async Task<bool> PutCategoryImage([FromQuery]int categoryId,[FromForm] IFormFile file)
        => await _dbContextWrapper.UpdateCategory(new Category(categoryId, file));
}