using Frontend.Models;

namespace Frontend.Services;

public interface IProductService
{
    Task<Product?> GetProductByIdAsync(string id);
    Task<Product?> GetProductByNameAsync(string name);
    Task<List<Product>?> GetAllProductsAsync();
    Task<List<Review>?> GetReviewsByProductIdAsync(string id);
    Task<bool> AddReviewAsync(string id, Review review);
    Task<bool> AddProduct(Product cart);
    Task<bool> UpdateProduct(string id,Product product);
    Task<bool> DeleteProduct(string id);
}