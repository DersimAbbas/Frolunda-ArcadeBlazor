namespace Frontend.Models
{
    public class Order
    {
        public string Id { get; set; }

        public User User { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
