using Frontend.Models;

namespace Frontend.Services;

public interface ILocalCartStorageService
{
    Dictionary<string, int> Cart { get; }
    event Action? OnChange;

    Task ClearCartAsync();
    Task LoadCartAsync();
    Task AddToCart(Product product);
    Task SaveCartAsync();
    int GetCartItemCount();
    decimal GetCartCost(List<Product> allProducts);
}
