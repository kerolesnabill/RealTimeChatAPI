namespace RealTimeChatAPI.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string? Image { get; set; }
    public string? About { get; set; }
    public DateTime CreatedAt { get; set; }
}