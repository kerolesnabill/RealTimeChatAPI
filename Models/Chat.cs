namespace RealTimeChatAPI.Models;

public class Chat
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool? IsGroupChat { get; set; }

    public ICollection<ChatUser> ChatUsers { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}
