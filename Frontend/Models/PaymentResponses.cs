namespace Frontend.Models;

public class PaymentIntentResponse
{
    public bool IsSuccess { get; set; }
    public bool RequiresAction { get; set; }
    public string? ClientSecret { get; set; }
    public string? ErrorMessage { get; set; }
}
public class ConfirmResponse
{
    public bool Success { get; set; }
}