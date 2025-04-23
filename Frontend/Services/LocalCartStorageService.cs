using Frontend.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Frontend.Services;

public class LocalCartStorageService : ILocalCartStorageService
{
    private readonly ProtectedLocalStorage _storage;
    private const string CartKey = "cart";

    public Dictionary<string, int> Cart { get; private set; } = new();

    public event Action? OnChange;

    public LocalCartStorageService(ProtectedLocalStorage storage)
    {
        _storage = storage;
    }

    public async Task LoadCartAsync()
    {
        var result = await _storage.GetAsync<Dictionary<string, int>>(CartKey);
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
        await _storage.SetAsync(CartKey, Cart);
        OnChange?.Invoke();
    }

    public int GetCartItemCount() => Cart.Values.Sum();
}
