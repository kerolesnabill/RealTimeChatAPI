namespace RealTimeChatAPI.Models;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReadAt { get; set; }

    public Guid SenderId { get; set; }
    public User Sender { get; set; } = default!;
    
    public Guid RecipientId { get; set; }
    public User Recipient { get; set; } = default!;
}

