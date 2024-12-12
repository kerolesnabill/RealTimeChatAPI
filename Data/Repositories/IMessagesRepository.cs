using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

public interface IMessagesRepository
{
    Task<Message> CreateAsync(Message message);
}
