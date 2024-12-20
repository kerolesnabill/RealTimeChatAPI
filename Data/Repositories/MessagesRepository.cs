using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<Message>> GetMessagesAsync(Guid userId1, Guid userId2)
    {
        return await dbContext.Messages
                .Where(m => 
                   (m.SenderId == userId1 && m.RecipientId == userId2) ||
                   (m.SenderId == userId2 && m.RecipientId == userId1))
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
    }
}
