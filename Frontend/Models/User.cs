namespace Frontend.Models
{
    public class User
    {
        public string? Id { get; set; }

        public string Email { get; set; }  //Adding this for now to test admin page functionality

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        // public string? Password { get; set; }

        public string Role { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}
