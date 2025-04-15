using Frontend.Models;

namespace Frontend.DummyData
{
    public class CreateProductDummyData
    {
        public static List<Product> GetDummyProducts()
        {
            var dummyUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Doe",
                Password = "temp123",
                Role = "Customer",
                PhoneNumber = "123-456-7890",
                Address = "123 Elm Street"
            };

            var product1 = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Mystic Quest",
                Description = "An epic adventure through ancient lands.",
                Price = 29.99,
                Genre = "RPG",
                Status = "Available",
                ImageLink = "https://via.placeholder.com/60",
                AgeRating = 12,
            };

            var product2 = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Battle Forge",
                Description = "A fast-paced strategy game.",
                Price = 19.99,
                Genre = "Strategy",
                Status = "Out of Stock",
                ImageLink = "https://via.placeholder.com/60",
                AgeRating = 16,
            };

            var product3 = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Pixel Runner",
                Description = "A retro-style endless runner.",
                Price = 9.99,
                Genre = "Arcade",
                Status = "Available",
                ImageLink = "https://via.placeholder.com/60",
                AgeRating = 3,
            };

            var review1 = new Review
            {
                Id = Guid.NewGuid().ToString(),
                Product = new CartProductDto { Id = product1.Id, Name = product1.Name, Price = product1.Price },
                User = dummyUser,
                Title = "Awesome Game",
                Comment = "Had a great time playing it!",
                Ratings = 5
            };

            var review2 = new Review
            {
                Id = Guid.NewGuid().ToString(),
                Product = new CartProductDto { Id = product2.Id, Name = product2.Name, Price = product2.Price },
                User = dummyUser,
                Title = "Challenging but Fun",
                Comment = "Tough levels but rewarding gameplay.",
                Ratings = 4
            };

            product1.Reviews.Add(review1);
            product2.Reviews.Add(review2);

            return new List<Product> { product1, product2, product3 };
        }

    }
}
