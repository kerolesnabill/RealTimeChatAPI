namespace RealTimeChatAPI.DTOs;

public class ChatDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool? IsGroupChat { get; set; }
    public string? Image { get; set; }
}
