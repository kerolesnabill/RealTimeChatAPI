namespace RealTimeChatAPI.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string HashedPassword { get; set; } = default!;
    public string? Image { get; set; }
    public string? About { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Message> SentMessages { get; set; } = [];
    public ICollection<Message> ReceivedMessages { get; set; } = [];
}
