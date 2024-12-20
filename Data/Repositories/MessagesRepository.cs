using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

internal class MessagesRepository(RealTimeChatDbContext dbContext) : IMessagesRepository
{
    public async Task<Message> AddAsync(Message message)
    {
        dbContext.Messages.Add(message);
        await dbContext.SaveChangesAsync();
        return message;
    }
}
