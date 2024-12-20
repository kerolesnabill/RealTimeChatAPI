namespace RealTimeChatAPI.DTOs;

public class ChatRoomDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Image { get; set; } = default!;
    public string LastMessage { get; set; } = default!; 
    public DateTime LastMessageTime { get; set; }
    public int UnreadMessagesCount { get; set; }
}
