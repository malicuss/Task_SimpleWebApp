using SimpleWebApp.Models;

namespace SimpleWebApp.Helpers;

public interface IDbContextWrapper
{
    List<Category> GetCategoriesFromDb();
    Task<Category> GetCategoryFromDb(int categoryId);
    List<Supplier> GetSuppliersFromDb();
    Task<Supplier> GetSupplierFromDb(int supplierId);
    List<Product> GetProductsFromDb(int numberOfProducts);
    Task<Product> GetProductFromDb(int productId);
    Task<Product> ProductToAddOrUpdate(int productId);
    Task<bool> AddOrUpdateProduct(Product p);
    Task<bool> AddUpdateCategory(Category p);
}