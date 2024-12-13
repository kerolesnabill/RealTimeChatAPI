namespace RealTimeChatAPI.DTOs;

public class ChatDto
{
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public bool? IsGroupChat { get; set; }
        public string? Image { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
}
