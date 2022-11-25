using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Models;

namespace SimpleWebApp.Helpers;

public class DbContextWrapper : IDbContextWrapper
{
    private readonly ILogger<DbContextWrapper> _logger;
    private readonly NorthwindContext _context;

    public DbContextWrapper(
        ILogger<DbContextWrapper> logger,
        NorthwindContext context )
    {
        _logger = logger;
        _context = context;
    }

   public List<Category> GetCategoriesFromDb()
       => _context.Categories.ToList();
   
   public async Task<Category> GetCategoryFromDb(int categoryId)
   {
       var res = await _context.Categories.FirstOrDefaultAsync(
           x=>x.CategoryId == categoryId);
       if (res == null)
           throw new CategoryNotFoundException($"No Category with such id {categoryId}");
       return res;
   }

    public List<Supplier> GetSuppliersFromDb()
        => _context.Suppliers.ToList();

    public async Task<Supplier> GetSupplierFromDb(int supplierId)
    {
        var res = await _context.Suppliers.FirstOrDefaultAsync(
            x=>x.SupplierId == supplierId);
        if (res == null)
            throw new SupplierNotFoundException($"No supplier with such id {supplierId}");
        return res;
    }

    public List<Product> GetProductsFromDb(int numberOfProducts = 0)
        => _context.Products.Take(numberOfProducts).ToList();

    public async Task<Product> GetProductFromDb(int productId)
    {
        var res = await _context.Products.FirstOrDefaultAsync(
            x=>x.ProductId == productId);
        if (res == null)
            throw new ProductNotFoundException($"No product found with such id:{productId}");
        return res;
    }

    public async Task<Product> ProductToAddOrUpdate(int productId)
    {
        Product res;
        try
        {
            res = await GetProductFromDb(productId);
        }
        catch (ProductNotFoundException e)
        {
            res = new Product { ProductId = 0 };
        }
        UpdateDependantProperties(res);

        return res;
    }

    public async Task<bool> AddOrUpdateProduct(Product _product)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x=>x.ProductId==_product.ProductId);
        if (product == null)
        {
            try
            {
                AddProduct(_product);
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"Unsuccessful adding product with id:{_product.ProductId.ToString()}");
                return false;
            }
        }
        else
        {
            try
            {
                UpdateProduct(_product);
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"Unsuccessful updating product with id:{_product.ProductId.ToString()}");
                return false;
            }
        }

        return true;

    }
    
    public async Task<bool> AddUpdateCategory(Category cat)
    {
        var res = await  _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == cat.CategoryId);
        if (res == null)
            return false;
        try
        {
            res.UpdateCategory(cat);
            _context.Categories.Update(res);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            _logger.LogError(e,$"Unsuccessful updating category with id:{cat.CategoryId}");
            return false;
        }

        return true;
    }

    private void AddProduct(Product product)
    { 
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    private void UpdateProduct(Product product)
    {
        var productToUpdate =_context.Products.FirstOrDefaultAsync(x => x.ProductId == product.ProductId).GetAwaiter().GetResult();
        productToUpdate.UpdateProduct(product);
        _context.Products.Update(productToUpdate);
        _context.SaveChanges();
    }

    private void UpdateDependantProperties(Product p)
    {
        foreach (var cat in GetCategoriesFromDb())
            p.Categories.Add(new SelectListItem(cat.CategoryName,cat.CategoryId.ToString())); 
        foreach (var sup in GetSuppliersFromDb()) 
            p.Suppliers.Add(new SelectListItem(sup.CompanyName, sup.SupplierId.ToString()));
    }
}