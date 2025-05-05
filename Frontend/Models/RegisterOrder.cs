namespace Frontend.Models
{
    public class RegisterOrder
    {
        public string UserId { get; set; } = null!;
        public Dictionary<string, int> Products {get; set; }
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }    
        public string Status { get; set; }
    }
}
