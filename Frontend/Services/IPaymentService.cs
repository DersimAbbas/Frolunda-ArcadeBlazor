using Frontend.Models;

namespace Frontend.Services;

public interface IPaymentService
{
    Task<PaymentIntentResponse> ProcessPayment(string paymentMethodId, decimal amount);
    Task<bool> ConfirmPayment(string clientSecret);
}
