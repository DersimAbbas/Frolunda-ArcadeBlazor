namespace Frontend.Models
{
    public class Product
    {
        public string Id { get; set; } = "";

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public double Price { get; set; }
        
        // public string Category { get; set; } (optional)
        public string Genre { get; set; }

        public string Status { get; set; } = "available";
        
        public string ImageLink { get; set; }
        
        public int AgeRating { get; set; }
        
        public List<Review> Reviews { get; set; } = new List<Review>();
        
        
    }
}
