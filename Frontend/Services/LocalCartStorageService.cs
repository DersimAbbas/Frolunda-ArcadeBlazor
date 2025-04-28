using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Frontend.Services;

public class LocalCartStorageService(ProtectedLocalStorage storage) : ILocalCartStorageService
{
    private const string CartKey = "cart";

    public Dictionary<string, int> Cart { get; private set; } = new();

    public event Action? OnChange;

    public async Task LoadCartAsync()
    {
        var result = await storage.GetAsync<Dictionary<string, int>>(CartKey);
        Cart = result.Success ? result.Value : new Dictionary<string, int>();
    }

    public async Task AddToCart(Product product)
    {
        if (Cart.ContainsKey(product.Id))
        {
            Cart[product.Id]++;
        }
        else
        {
            Cart[product.Id] = 1;
        }

        await SaveCartAsync();
    }

    public async Task SaveCartAsync()
    {
        await storage.SetAsync(CartKey, Cart);
        OnChange?.Invoke();
    }

    public async Task RemoveFromCart(CartProductDto product)
    {
        if (Cart.ContainsKey(product.Id))
        {
            Cart.Remove(product.Id);
            await SaveCartAsync();
        }
    }

    public async Task ClearCartAsync()
    {
        Cart.Clear();
        await SaveCartAsync();
    }

    public int GetCartItemCount() => Cart.Values.Sum();

    public decimal GetCartCost(List<Product> allProducts)
    {
        return Cart.Sum(item =>
        {
            var product = allProducts.FirstOrDefault(p => p.Id == item.Key);
            return product != null ? (decimal)product.Price * item.Value : 0;
        });
    }
}