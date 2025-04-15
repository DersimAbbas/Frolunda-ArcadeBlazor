namespace Frontend.Models
{
    public class Review
    {
        public string Id { get; set; }

        public CartProductDto Product { get; set; }

        public User User { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }

        public int Ratings { get; set; }
        // ?? public DateTime ReviewDate { get; set;} = DateTime.Now();

    }
}
