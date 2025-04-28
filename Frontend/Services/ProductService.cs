using Frontend.Models;

namespace Frontend.Services;

public class ProductService(HttpClient httpClient) : IProductService
{
    public async Task<Product?> GetProductByIdAsync(string id)
    {
        return await httpClient.GetFromJsonAsync<Product>($"api/products/{id}");
    }

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await httpClient.GetFromJsonAsync<Product>($"api/products/{name}");
    }

    public async Task<List<Product>?> GetAllProductsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<List<Product>>("api/products");
        return response ?? new List<Product>();
    }

    public async Task<List<Review>?> GetReviewsByProductIdAsync(string id)
    {
        return await httpClient.GetFromJsonAsync<List<Review>>($"api/products/{id}/reviews/");
    }

    public async Task<bool> AddReviewAsync(string id, Review review)
    {
        var response = await httpClient.PostAsJsonAsync($"api/products/{id}/reviews", review);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddProduct(Product cart)
    {
        var response = await httpClient.PostAsJsonAsync("api/products", cart);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProduct(string id, Product product)
    {
        var response = await httpClient.PutAsJsonAsync($"api/products/{id}", product);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var response = await httpClient.DeleteAsync($"api/products/{id}");
        return response.IsSuccessStatusCode;
    }
}