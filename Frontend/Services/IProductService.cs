using Frontend.Models;

namespace Frontend.Services;

public interface IProductService
{
    Task<Product?> GetProductByIdAsync(string id);
    Task<List<Product>?> GetAllProductsAsync();
    Task<bool> AddProduct(Product cart);
    Task<bool> UpdateProduct(string id,Product product);
    Task<bool> DeleteProduct(string id);
}