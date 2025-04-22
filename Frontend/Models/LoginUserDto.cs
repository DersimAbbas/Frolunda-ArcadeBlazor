using System.ComponentModel.DataAnnotations;

namespace Frontend.Models;

public class LoginUserDto
{
    [Required, EmailAddress]
    public required string Email { get; set; }
    [Required]
    [MinLength(6)]
    public required string Password { get; set; }
}