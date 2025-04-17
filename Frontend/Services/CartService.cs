using Frontend.Models;

namespace Frontend.Services;

public class CartService(HttpClient httpClient) : ICartService
{
    public async Task<Cart?> GetCartByIdAsync(string id)
    {
       return await httpClient.GetFromJsonAsync<Cart>("api/carts/" + id);
    }

    public async Task<List<Cart>?> GetAllCartsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Cart>>("api/carts");
    }

    public async Task<bool> AddCart(Cart cart)
    {
        var result = await httpClient.PostAsJsonAsync("api/carts", cart);
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCart(string id, Cart cart)
    {
        var result = await httpClient.PutAsJsonAsync($"api/carts/{id}", cart);
        return result.IsSuccessStatusCode;
    }
    
    public async Task<bool> DeleteCart(string id)
    {
        var result = await httpClient.DeleteAsync($"api/carts/{id}"); 
        return result.IsSuccessStatusCode;
    }
}