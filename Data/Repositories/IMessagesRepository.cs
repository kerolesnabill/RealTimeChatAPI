using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

public interface IMessagesRepository
{
    Task<Message> AddAsync(Message message);
    Task<Message?> GetByIdAsync(Guid id);
    Task<Message> UpdateAsync(Message message);
    Task<IEnumerable<Message>> GetMessagesAsync(Guid userId1,Guid userId2);
    Task<IEnumerable<ChatRoomDto>> GetChatRoomsAsync(Guid userId);
    Task<IEnumerable<Message>> ReadUnreadMessagesAsync(Guid userId, Guid userChatId);
}
