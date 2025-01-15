using System.ComponentModel.DataAnnotations;

namespace Phaedra.Server.Models.Entities.Users;

public class User
{
    public int Id { get; set; }
    [Required]
    [MinLength(4, ErrorMessage = "Username must be at least 4 characters long.")]
    [MaxLength(50, ErrorMessage = "Username must be between 4 and 50 characters")]
    public string Username { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}