using Microsoft.EntityFrameworkCore;
using RealTimeChatAPI.DTOs;
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

    public async Task<Chat?> GetByIdWithMessagesAsync(Guid id)
    {
        return await dbContext.Chats
            .Include(c => c.Messages).SingleOrDefaultAsync(c => c.Id == id);
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

    public async Task<IEnumerable<ChatDto>> GetChatsByUserIdAsync(Guid userId)
    {
        return await dbContext.ChatUsers
            .Where(cu => cu.UserId == userId)
            .Select(cu => new ChatDto
            {
                Id = cu.ChatId,
                IsGroupChat = cu.Chat.IsGroupChat,
                Name = cu.Chat.IsGroupChat == true ? cu.Chat.Name : 
                    cu.Chat.ChatUsers.SingleOrDefault(cu => cu.UserId != userId)!.User.Name,
                Image = cu.Chat.IsGroupChat == true ? cu.Chat.Image :
                    cu.Chat.ChatUsers.SingleOrDefault(cu => cu.UserId != userId)!.User.Image,
                LastMessage = cu.Chat.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault().Content,
                LastMessageTime = cu.Chat.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault().CreatedAt,
            }).ToListAsync();
    }

    public async Task<bool> IsAChatMember(Guid chatId, Guid userId)
    {
        var chatUser = await dbContext.ChatUsers
            .Where(cu => cu.ChatId == chatId && cu.UserId == userId)
            .FirstOrDefaultAsync();
        return chatUser != null;
    }
}
