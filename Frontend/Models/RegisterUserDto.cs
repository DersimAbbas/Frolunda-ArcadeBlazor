using System.ComponentModel.DataAnnotations;

namespace Frontend.Models;

public class RegisterUserDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [MinLength(1)]
    public required string FirstName { get; set; }
    
    [Required]
    [MinLength(1)]
    public required string LastName { get; set; }
    
    [Required]
    [MinLength(3)]
    public required string Username { get; set; }
    
    [Required]
    [MinLength(1)]
    public required string Address { get; set; }
    
    [Required]
    [MinLength(6)]
    public required string Password { get; set; }
    
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }

    public string Role { get; set; } = "user";

    // public string Role { get; set; }
}