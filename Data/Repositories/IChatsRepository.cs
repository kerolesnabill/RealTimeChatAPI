using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

public interface IChatsRepository
{
    Task<Chat> CreateAsync(Chat chat);
    Task<Chat?> GetByUsersAsync(Guid firstUserId, Guid secondUserId);
}
