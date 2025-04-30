
namespace Frontend.Models
{
    public class Cart
    {
        public string Id { get; set; } = string.Empty;
        
        public string UserId { get; set; }
        //public List<CartProductDto> CartItems { get; set; }

        public Dictionary<string, int> cartItems { get; set; } 
    }   
}
