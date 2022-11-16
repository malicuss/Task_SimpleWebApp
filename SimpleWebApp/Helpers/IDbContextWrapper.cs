using SimpleWebApp.Models;

namespace SimpleWebApp.Helpers;

public interface IDbContextWrapper
{
    Task<List<Category>> GetCategoriesFromDb();
    Task<Category> GetCategoryFromDb(int categoryId);
    List<Supplier> GetSuppliersFromDb();
    Task<Supplier> GetSupplierFromDb(int supplierId);
    Task<List<Product>> GetAllProductsFromDb();
    List<Product> GetProductsFromDb(int numberOfProducts);
    Task<Product> GetProductFromDb(int productId);
    Task<Product> ProductToAddOrUpdate(int productId);
    Task<bool> AddOrUpdateProduct(Product p);
    Task<bool> DeleteProduct(Product p);
    Task<bool> AddUpdateCategory(Category p);
}