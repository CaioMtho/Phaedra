using System.ComponentModel.DataAnnotations;

namespace Phaedra.Server.Models.DTO.Users.User;

public class CreateUserDto
{
    [Required]
    public string Username { get; init; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;
    [Required]
    public string Password { get; init; } = null!;

}