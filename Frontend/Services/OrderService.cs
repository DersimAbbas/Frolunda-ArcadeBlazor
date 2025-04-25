using Frontend.Models;
using Frontend.Services.Interfaces;

namespace Frontend.Services;

public class OrderService(HttpClient httpClient) : IOrderService
{
    public async Task<Order?> GetOrderByIdAsync(string id)
    {
        return await httpClient.GetFromJsonAsync<Order>($"api/orders/{id}");
    }

    public async Task<List<Order>?> GetAllOrderAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Order>>("api/orders");
    }

    public async Task<List<Order>?> GetOrdersByUserIdAsync(string id)
    {
        return await httpClient.GetFromJsonAsync<List<Order>>("api/orders/user/" + id);
    }

    public async Task<bool> AddOrder(Order order)
    {
        var result = await httpClient.PostAsJsonAsync("api/orders", order);
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateOrder(string id, Order order)
    {
        var result = await httpClient.PutAsJsonAsync($"api/orders/{id}", order);
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteOrder(string id)
    {
        var result = await httpClient.DeleteAsync($"api/orders/{id}");
        return result.IsSuccessStatusCode;
    }
}