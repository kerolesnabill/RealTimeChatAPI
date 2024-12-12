using Microsoft.EntityFrameworkCore;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data.Repositories;

internal class ChatsRepository(RealTimeChatDbContext dbContext) : IChatsRepository
{
    public async Task<Chat> CreateAsync(Chat chat)
    {
        dbContext.Chats.Add(chat);
        await dbContext.SaveChangesAsync();
        return chat;
    }

    public async Task<Chat?> GetByIdAsync(Guid id)
    {
        return await dbContext.Chats
            .Include(c => c.ChatUsers).SingleOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Chat?> GetByUsersAsync(Guid firstUserId, Guid secondUserId)
    {
        return await dbContext.Chats
                .Where(c =>
                    (c.ChatUsers.Order().First().UserId == firstUserId ||
                     c.ChatUsers.Order().First().UserId == secondUserId)
                     &&
                    (c.ChatUsers.Order().Last().UserId == firstUserId ||
                     c.ChatUsers.Order().Last().UserId == secondUserId)
                     &&
                     (c.IsGroupChat == null || c.IsGroupChat == false)
                     ).FirstOrDefaultAsync();
    }
}
