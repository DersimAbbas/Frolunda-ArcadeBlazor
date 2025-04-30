using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.JSInterop;
using System.Text.Json;


namespace Frontend.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    private const string _productsKey = "products";
    private const string _timestampKey = "productsTimestamp";

    public ProductService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this._httpClient = httpClient;
        this._jsRuntime = jsRuntime;
    }

    public async Task<Product?> GetProductByIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<Product>($"api/products/{id}");
    }

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await _httpClient.GetFromJsonAsync<Product>($"api/products/{name}");
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _productsKey);
        var timestampString = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _timestampKey);

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
                catch
                {
                    
                }
            }
        }

        var products = await _httpClient.GetFromJsonAsync<List<Product>>("api/products") ?? new List<Product>();

        var productJson = JsonSerializer.Serialize(products);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _productsKey, productJson);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _timestampKey, DateTime.UtcNow.Ticks.ToString());

        return products;
    }

    public async Task<List<Review>?> GetReviewsByProductIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<List<Review>>($"api/products/{id}/reviews/");
    }

    public async Task<bool> AddReviewAsync(string id, Review review)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/products/{id}/reviews", review);

        var product = await GetProductByIdAsync(id);
        if (response.IsSuccessStatusCode)
        {
            var productJson = JsonSerializer.Serialize(product);
            await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _productsKey, productJson);
        }

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddProduct(Product product)
    {
        var response = await _httpClient.PostAsJsonAsync("api/products", product);
        if (!response.IsSuccessStatusCode)
            return false;

        var existingJson = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _productsKey);
        var products = string.IsNullOrWhiteSpace(existingJson)
            ? new List<Product>()
            : JsonSerializer.Deserialize<List<Product>>(existingJson)!;

        products.Add(product);

        var updatedJson = JsonSerializer.Serialize(products);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _productsKey, updatedJson);

        return true;
    }

    public async Task<bool> UpdateProduct(string id, Product product)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", product);
        if (!response.IsSuccessStatusCode)
            return false;

        var existingJson = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _productsKey);
        var products = string.IsNullOrWhiteSpace(existingJson)
            ? new List<Product>()
            : JsonSerializer.Deserialize<List<Product>>(existingJson)!;

        var index = products.FindIndex(p => p.Id == id);
        if (index != -1)
        {
            products[index] = product;
        }

        var updatedJson = JsonSerializer.Serialize(products);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _productsKey, updatedJson);

        return true;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var response = await _httpClient.DeleteAsync($"api/products/{id}");

        if (!response.IsSuccessStatusCode)
            return false;

        var existingJson = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _productsKey);
        var products = string.IsNullOrWhiteSpace(existingJson)
            ? new List<Product>()
            : JsonSerializer.Deserialize<List<Product>>(existingJson)!;

        var productToRemove = products.FirstOrDefault(p => p.Id == id);
        if (productToRemove != null)
        {
            products.Remove(productToRemove);
            var updatedJson = JsonSerializer.Serialize(products);
            await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _productsKey, updatedJson);
        }

        return true;
    }
}