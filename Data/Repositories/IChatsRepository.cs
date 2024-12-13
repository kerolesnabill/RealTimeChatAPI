using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

public interface IChatsRepository
{
    Task<Chat> CreateAsync(Chat chat);
    Task<Chat?> GetByUsersAsync(Guid firstUserId, Guid secondUserId);
    Task<Chat?> GetByIdAsync(Guid id);
    Task<Chat?> GetByIdWithMessagesAsync(Guid id);
    Task<IEnumerable<ChatDto>> GetChatsByUserIdAsync(Guid userId);
    Task<bool> IsAChatMember(Guid chatId, Guid userId);
}
