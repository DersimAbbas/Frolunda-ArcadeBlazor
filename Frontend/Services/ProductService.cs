using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.JSInterop;
using System.Text.Json;


namespace Frontend.Services;

public class ProductService : IProductService
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    private const string ProductsKey = "products";
    private const string TimestampKey = "productsTimestamp";

    public ProductService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }

    public async Task<Product?> GetProductByIdAsync(string id)
    {
        return await httpClient.GetFromJsonAsync<Product>($"api/products/{id}");
    }

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await httpClient.GetFromJsonAsync<Product>($"api/products/{name}");
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        var json = await jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", ProductsKey);
        var timestampString = await jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", TimestampKey);

        if (!string.IsNullOrEmpty(json) && long.TryParse(timestampString, out var ticks))
        {
            var savedTime = new DateTime(ticks);
            if ((DateTime.UtcNow - savedTime).TotalHours < 24)
            {
                try
                {
                    var cachedProducts = JsonSerializer.Deserialize<List<Product>>(json);
                    if (cachedProducts != null)
                        return cachedProducts;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error deserializing cached products: {ex.Message}");
                }
            }
        }

        var products = await httpClient.GetFromJsonAsync<List<Product>>("api/products") ?? new List<Product>();

        var productJson = JsonSerializer.Serialize(products);
        await jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", ProductsKey, productJson);
        await jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", TimestampKey, DateTime.UtcNow.Ticks.ToString());

        return products;
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