using Firebase.Auth;
using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;
using static Frontend.Components.User.Components.ContactSupportModal;

namespace Frontend.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private const string _LogicAppUrl = "https://prod-09.swedencentral.logic.azure.com:443/workflows/5bc4a01e3d894d27a245f165c499a5be/triggers/When_a_HTTP_request_is_received/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2FWhen_a_HTTP_request_is_received%2Frun&sv=1.0&sig=Vu1fG36sd0LpBqoGxzhQkTgIOF_NtlNyUxOf5khyRsE";
    private const string _ordersKey = "orders";
    private const string _timestampKey = "ordersTimestamp";
    private const string _FuncTrigger = "https://frolunda-arcadefunc.azurewebsites.net/api/get-orders";
    public OrderService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this._httpClient = httpClient;
        this._jsRuntime = jsRuntime;
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<Order>($"api/orders/{id}");
    }

    public async Task<List<RegisterOrder>?> GetAllOrdersAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _ordersKey);
        var timestampString = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _timestampKey);

        if (!string.IsNullOrEmpty(json) && long.TryParse(timestampString, out var ticks))
        {
            var savedTime = new DateTime(ticks);
            if ((DateTime.UtcNow - savedTime).TotalHours < 24)
            {
                try
                {
                    var cachedOrders = JsonSerializer.Deserialize<List<RegisterOrder>>(json);
                    if (cachedOrders != null)
                        return cachedOrders;
                }
                catch
                {

                }
            }
        }

        var orders = await _httpClient.GetFromJsonAsync<List<RegisterOrder>>(_FuncTrigger) ?? new List<RegisterOrder>();

        var orderJson = JsonSerializer.Serialize(orders);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _ordersKey, orderJson);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _timestampKey, DateTime.UtcNow.Ticks.ToString());

        return orders;
    }

    public async Task<List<RegisterOrder>?> GetOrdersByUserIdAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<RegisterOrder>>($"https://frolunda-arcadefunc.azurewebsites.net/api/get-order?userid={id}");
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching orders: {ex.Message}");
            return new List<RegisterOrder>();
        }

        
    }

    public async Task<bool> AddOrder(Order order)
    {
        var result = await _httpClient.PostAsJsonAsync("api/orders", order);
        if (!result.IsSuccessStatusCode)
            return false;

        var existingJson = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _ordersKey);
        var orders = string.IsNullOrWhiteSpace(existingJson)
            ? new List<Order>()
            : JsonSerializer.Deserialize<List<Order>>(existingJson)!;

        orders.Add(order);

        var updatedJson = JsonSerializer.Serialize(orders);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _ordersKey, updatedJson);

        return true;
    }

    public async Task<bool> UpdateOrder(string id, Order order)
    {
        var result = await _httpClient.PutAsJsonAsync($"api/orders/{id}", order);
        if (!result.IsSuccessStatusCode)
            return false;

        var existingJson = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _ordersKey);
        var orders = string.IsNullOrWhiteSpace(existingJson)
            ? new List<Order>()
            : JsonSerializer.Deserialize<List<Order>>(existingJson)!;

        var index = orders.FindIndex(o => o.Id == id);
        if (index != -1)
        {
            orders[index] = order;
        }

        var updatedJson = JsonSerializer.Serialize(orders);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _ordersKey, updatedJson);

        return true;
    }

    public async Task<bool> DeleteOrder(string id)
    {
        var result = await _httpClient.DeleteAsync($"api/orders/{id}");
        if (!result.IsSuccessStatusCode)
            return false;

        var existingJson = await _jsRuntime.InvokeAsync<string>("myLocalStorage.getItem", _ordersKey);
        var orders = string.IsNullOrWhiteSpace(existingJson)
            ? new List<Order>()
            : JsonSerializer.Deserialize<List<Order>>(existingJson)!;

        orders.RemoveAll(o => o.Id == id);

        var updatedJson = JsonSerializer.Serialize(orders);
        await _jsRuntime.InvokeVoidAsync("myLocalStorage.setItem", _ordersKey, updatedJson);

        return true;
    }

    public async Task<bool> DisputeOrder(string email, string name, string orderId)
    {

        var dispute = new SupportRequestModel
        {

            Name = name,
            Email = email,
            OrderId = orderId

        };
        var opts = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(dispute, opts);

        using var client = new HttpClient();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(_LogicAppUrl, content);
        return response.IsSuccessStatusCode;
    }


}