using System.ComponentModel.DataAnnotations;
using Phaedra.Server.Models.Shared;

namespace Phaedra.Server.Models.Entities.UserEntities;

public class User : IEntity
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}