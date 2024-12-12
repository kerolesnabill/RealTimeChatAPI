namespace RealTimeChatAPI.Models;

public class MessageStatus
{
    public Guid Id { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }

    public Guid MessageId { get; set; }
    public Message Message { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}

