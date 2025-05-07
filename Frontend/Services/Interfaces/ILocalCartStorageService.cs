using Frontend.Models;

namespace Frontend.Services.Interfaces;

public interface ILocalCartStorageService
{
    Dictionary<string, int> Cart { get; }
    event Action? OnChange;

    Task ClearCartAsync();
    Task LoadCartAsync();
    Task AddToCart(Product product);
    Task AddToCartByProductId(string id);
    Task SaveCartAsync();
    int GetCartItemCount();
    decimal GetCartCost(List<Product> allProducts);
}
