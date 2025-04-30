using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Frontend.Services;

public class CartService : ICartService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    private const string _cartsKey = "carts";
    private const string _timestampKey = "cartsTimestamp";

    public CartService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this._httpClient = httpClient;
        this._jsRuntime = jsRuntime;
    }

    public async Task<Cart?> GetCartByIdAsync(string id)
    {
       return await _httpClient.GetFromJsonAsync<Cart>("api/carts/" + id);
    }

    public async Task<List<Cart>?> GetAllCartsAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _cartsKey);
        var timestampString = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _timestampKey);

        if (!string.IsNullOrEmpty(json) && long.TryParse(timestampString, out var ticks))
        {
            var savedTime = new DateTime(ticks);
            if ((DateTime.UtcNow - savedTime).TotalHours < 24)
            {
                try
                {
                    var cachedCarts = JsonSerializer.Deserialize<List<Cart>>(json);
                    if (cachedCarts != null)
                        return cachedCarts;
                }
                catch
                {

                }
            }
        }

        var carts = await _httpClient.GetFromJsonAsync<List<Cart>>("api/carts") ?? new List<Cart>();

        var cartJson = JsonSerializer.Serialize(carts);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _cartsKey, cartJson);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _timestampKey, DateTime.UtcNow.Ticks.ToString());

        return carts;
    }

    public async Task<bool> AddCart(Cart cart)
    {
        var result = await _httpClient.PostAsJsonAsync("api/carts", cart);
        if (result.IsSuccessStatusCode)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _cartsKey, cartJson);
        }
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCart(string id, Cart cart)
    {
        var result = await _httpClient.PutAsJsonAsync($"api/carts/{id}", cart);
        if (result.IsSuccessStatusCode)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _cartsKey, cartJson);
        }
        return result.IsSuccessStatusCode;
    }
    
    public async Task<bool> DeleteCart(string id)
    {
        var cart = await GetCartByIdAsync(id);
        var result = await _httpClient.DeleteAsync($"api/carts/{id}");
        if (result.IsSuccessStatusCode)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _cartsKey, cartJson);
        }
        return result.IsSuccessStatusCode;
    }
}