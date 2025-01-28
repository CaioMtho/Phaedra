using System.ComponentModel.DataAnnotations;
using Phaedra.Server.Models.Shared;

namespace Phaedra.Server.Models.Entities.UserEntities;

public class User : IEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MinLength(4, ErrorMessage = "Username must be at least 4 characters long.")]
    [MaxLength(50, ErrorMessage = "Username must be between 4 and 50 characters")]
    public string Username { get; set; } = null!;
    [Required]
    [EmailAddress]
    [MaxLength(50, ErrorMessage = "Email too long")]
    public string Email { get; set; } = null!;
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [MaxLength(50, ErrorMessage = "Password too long")]
    public string Password { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}