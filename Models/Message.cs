namespace RealTimeChatAPI.Models;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool? IsDeleted { get; set; }

    public Guid SenderId { get; set; }
    public User Sender { get; set; } = default!;

    public Guid ChatId { get; set; }
    public Chat Chat { get; set; } = default!;

    public ICollection<MessageStatus> MessageStatuses { get; set; } = [];
}

