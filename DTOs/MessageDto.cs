using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public bool? IsDeleted { get; set; }
}
