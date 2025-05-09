using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class RegisterOrder
    {
        public string UserId { get; set; } = null!;
        public Dictionary<string, int> Products {get; set; }
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }    
        public string Status { get; set; }
        public string UserEmail { get; set; } = null!;
    }

    public class RegisterOrderDTO
    {
        [JsonPropertyName("userid")]
        public string UserId { get; set; } = null!;
        [JsonPropertyName("products")]
        public Dictionary<string, int> Products { get; set; }
        

        [JsonPropertyName("email")]
        public string UserEmail { get; set; } = null!;
    }
}
