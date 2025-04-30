
namespace Frontend.Models
{
    public class Cart
    {
        public string Id { get; set; }
        
        public User User { get; set; }

        //public List<CartProductDto> CartItems { get; set; }
        public Dictionary<string, int> CartItems { get; set; } 
    }   
}
