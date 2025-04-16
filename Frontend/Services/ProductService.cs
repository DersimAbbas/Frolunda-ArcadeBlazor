using Frontend.Models;

namespace Frontend.Services;

public class ProductService(HttpClient httpClient) : IProductService
{
    public async Task<Product?> GetProductByIdAsync(string id)
    {
        return await httpClient.GetFromJsonAsync<Product>($"api/products/{id}");
    }
    
    public async Task<List<Product>?> GetAllProductsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Product>>("https://localhost:44313/api/products");
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