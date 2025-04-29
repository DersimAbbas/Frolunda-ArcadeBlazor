namespace Frontend.Models
{
    public class RegisterOrder
    {
        public string UserId { get; set; } = null!;
        public Dictionary<string, int> Products {get; set; }
    }
}
