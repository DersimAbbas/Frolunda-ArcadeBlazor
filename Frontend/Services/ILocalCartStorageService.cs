using Frontend.Models;

namespace Frontend.Services;

public interface ILocalCartStorageService
{
    Dictionary<string, int> Cart { get; }
    event Action? OnChange;

    Task LoadCartAsync();
    Task AddToCart(Product product);
    Task SaveCartAsync();
    int GetCartItemCount();
}
