using System.ComponentModel.DataAnnotations;

namespace Phaedra.Server.DTO.User;

public class CreateUserDto
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;

}