namespace Phaedra.Server.DTO.User;

public class UserDto
{
    public int Id { get; init; }
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public DateTime CreatedAt { get; init; }

}