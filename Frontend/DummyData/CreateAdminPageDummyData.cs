using Frontend.Models;

namespace Frontend.DummyData
{
    public class CreateAdminPageDummyData
    {
        public static List<Product> GetDummyProducts()
        {
            var dummyUsers = GetDummyUsers();
            var dummyUser = dummyUsers.First();

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

        public static List<User> GetDummyUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = "1",
                    Email = "john.doe@gmail.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Password = "temp123",
                    Role = "user",
                    PhoneNumber = "123-456-7890",
                    Address = "123 Elm Street"
                },
                new User
                {
                    Id = "2",
                    Email = "jane.smith@gmail.com",
                    FirstName = "Jane",
                    LastName = "Smith",
                    Password = "securepass456",
                    Role = "admin",
                    PhoneNumber = "987-654-3210",
                    Address = "456 Oak Avenue"
                },
                new User
                {
                    Id = "3",
                    Email = "lebron.james@gmail.com",
                    FirstName = "Lebron",
                    LastName = "James",
                    Password = "basketball789",
                    Role = "user",
                    PhoneNumber = "978-654-3210",
                    Address = "789 Basketball Lane"
                },
                new User
                {
                    Id = "4",
                    Email = "lebron.james@gmail.com",
                    FirstName = "Lebron",
                    LastName = "James",
                    Password = "basketball789",
                    Role = "user",
                    PhoneNumber = "978-654-3210",
                    Address = "789 Basketball Lane"
                },
                new User
                {
                    Id = "5",
                    Email = "lebron.james@gmail.com",
                    FirstName = "Lebron",
                    LastName = "James",
                    Password = "basketball789",
                    Role = "user",
                    PhoneNumber = "978-654-3210",
                    Address = "789 Basketball Lane"
                },
                new User
                {
                    Id = "6",
                    Email = "lebron.james@gmail.com",
                    FirstName = "Lebron",
                    LastName = "James",
                    Password = "basketball789",
                    Role = "user",
                    PhoneNumber = "978-654-3210",
                    Address = "789 Basketball Lane"
                },
                new User
                {
                    Id = "7",
                    Email = "lebron.james@gmail.com",
                    FirstName = "Lebron",
                    LastName = "James",
                    Password = "basketball789",
                    Role = "user",
                    PhoneNumber = "978-654-3210",
                    Address = "789 Basketball Lane"
                },
                new User
                {
                    Id = "8",
                    Email = "lebron.james@gmail.com",
                    FirstName = "Lebron",
                    LastName = "James",
                    Password = "basketball789",
                    Role = "user",
                    PhoneNumber = "978-654-3210",
                    Address = "789 Basketball Lane"
                },
                new User
                {
                    Id = "9",
                    Email = "lebron.james@gmail.com",
                    FirstName = "Lebron",
                    LastName = "James",
                    Password = "basketball789",
                    Role = "user",
                    PhoneNumber = "978-654-3210",
                    Address = "789 Basketball Lane"
                }

            };
        }

        public static List<Order> GetDummyOrders()
        {
            var users = GetDummyUsers();
            var products = GetDummyProducts();

            var order1 = new Order
            {
                Id = Guid.NewGuid().ToString(),
                User = users[0],
                Products = new List<Product> { products[0], products[1] }
            };

            var order2 = new Order
            {
                Id = Guid.NewGuid().ToString(),
                User = users[1],
                Products = new List<Product> { products[2] }
            };

            var order3 = new Order
            {
                Id = Guid.NewGuid().ToString(),
                User = users[2],
                Products = new List<Product> { products[0], products[2] }
            };
            var order4 = new Order
            {
                Id = Guid.NewGuid().ToString(),
                User = users[0],
                Products = new List<Product> { products[0], products[1] }
            };
            var order5 = new Order
            {
                Id = Guid.NewGuid().ToString(),
                User = users[0],
                Products = new List<Product> { products[0], products[1] }
            };

            return new List<Order> { order1, order2, order3, order4, order5 };
        }

        public static List<Cart> GetDummyCarts()
        {
            var users = CreateAdminPageDummyData.GetDummyUsers();
            var products = CreateAdminPageDummyData.GetDummyProducts();

            var cart1 = new Cart
            {
                Id = Guid.NewGuid().ToString(),
                User = users[0],
                CartItems = new List<CartProductDto>
                {
                    new CartProductDto { Id = products[0].Id, Name = products[0].Name, Price = products[0].Price },
                    new CartProductDto { Id = products[2].Id, Name = products[2].Name, Price = products[2].Price }
                }
            };

            var cart2 = new Cart
            {
                Id = Guid.NewGuid().ToString(),
                User = users[1],
                CartItems = new List<CartProductDto>
                {
                    new CartProductDto { Id = products[1].Id, Name = products[1].Name, Price = products[1].Price }
                }
            };

            return new List<Cart> { cart1, cart2 };
        }
    }
}