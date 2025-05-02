namespace Frontend.Models
{
    public class User
    {
        public string? Id { get; set; }

        public string Email { get; set; } = string.Empty;  //Adding this for now to test admin page functionality

        public string? FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; } = string.Empty;

        // public string? Password { get; set; }

        public string Role { get; set; } = "admin";

        public string? PhoneNumber { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;
    }
}
