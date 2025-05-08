using Frontend.Models;

namespace Frontend.Services.Interfaces;

public interface ICartService
{
    Task<Cart?> GetCartByIdAsync(string id);
    Task<Cart?> GetCartByUserIdAsync(string userId);
    Task<List<Cart>?> GetAllCartsAsync();
    Task<bool> AddCart(Cart cart);
    Task<bool> UpdateCart(string id,Cart cart);
    Task<bool> DeleteCart(string id);
    Task<bool> DeleteCartByUserId(string userId);
}