using Frontend.Models;

namespace Frontend.Services;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;

    public PaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaymentIntentResponse> ProcessPayment(string paymentMethodId, decimal amount)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/payment", new
        {
            PaymentMethodId = paymentMethodId,
            Amount = amount
        });

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<PaymentIntentResponse>();
            data.IsSuccess = true;
            return data!;
        }

        var errorContent = await response.Content.ReadAsStringAsync();

        return new PaymentIntentResponse
        {
            IsSuccess = false,
            ErrorMessage = errorContent
        };
    }

    public async Task<bool> ConfirmPayment(string clientSecret)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/payment/confirm", new { ClientSecret = clientSecret });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ConfirmResponse>();
            return result?.Success == true;
        }

        return false;
    }
}