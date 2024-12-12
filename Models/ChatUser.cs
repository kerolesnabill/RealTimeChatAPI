namespace RealTimeChatAPI.Models;

public class ChatUser
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid ChatId { get; set; }
    public Chat Chat { get; set; } = default!;

    public bool? IsAdmin { get; set; }
    public DateTime? JoinedAt { get; set; }
}
