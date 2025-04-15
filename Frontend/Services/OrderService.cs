using Frontend.Models;

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