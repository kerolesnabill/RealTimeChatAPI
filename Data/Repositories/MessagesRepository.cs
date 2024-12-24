using Microsoft.EntityFrameworkCore;
using RealTimeChatAPI.DTOs;
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

    public async Task<Message?> GetByIdAsync(Guid id)
    {
        return await dbContext.Messages.SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Message> UpdateAsync(Message message)
    {
        dbContext.Update(message);
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

    public async Task<IEnumerable<ChatRoomDto>> GetChatRoomsAsync(Guid userId)
    {
        var chatRooms = await dbContext.Messages
            .Where(m => m.SenderId == userId || m.RecipientId == userId)
            .Include(m => m.Sender)
            .Include(m => m.Recipient)
            .GroupBy(m => new
            {
                Participant1 = m.SenderId < m.RecipientId ? m.SenderId : m.RecipientId,
                Participant2 = m.SenderId < m.RecipientId ? m.RecipientId : m.SenderId
            })
            .Select(g => new
            {
                g.Key.Participant1,
                g.Key.Participant2,
                LatestMessage = g.OrderByDescending(m => m.CreatedAt).FirstOrDefault(),
                UnreadMessagesCount = g.Count(m => m.SenderId != userId && m.ReadAt == null)
            })
            .ToListAsync();

        var chatRoomDtos = chatRooms.Select(x =>
        {
            var isUserParticipant1 = x.LatestMessage!.SenderId == userId;
            var otherParticipantId = x.Participant1 == userId? x.Participant2 : x.Participant1;
            var latestMessage = x.LatestMessage;

            return new ChatRoomDto
            {
                UserId = otherParticipantId,
                Username = isUserParticipant1 ? latestMessage.Recipient.Username : latestMessage.Sender.Username,
                Name = isUserParticipant1 ? latestMessage.Recipient.Name : latestMessage.Sender.Name,
                Image = isUserParticipant1 ? latestMessage.Recipient.Image : latestMessage.Sender.Image,
                LastMessage = latestMessage.Content,
                LastMessageTime = latestMessage.CreatedAt,
                UnreadMessagesCount = x.UnreadMessagesCount
            };
        }).ToList();

        return chatRoomDtos;
    }

    public async Task<IEnumerable<Message>> ReadUnreadMessagesAsync(Guid userId, Guid userChatId)
    {
        var messages = await dbContext.Messages
            .Where(m => m.RecipientId == userId && m.SenderId == userChatId && m.ReadAt == null)
            .ToListAsync();

        foreach (var message in messages)
            message.ReadAt = DateTime.UtcNow;

        dbContext.Messages.UpdateRange(messages);
        await dbContext.SaveChangesAsync();
        return messages;
    }

    public async Task<IEnumerable<Message>> DeliveredAllMessagesAsync(Guid userId)
    {
        var messages = await dbContext.Messages
            .Where(m => m.RecipientId == userId && m.DeliveredAt == null)
            .ToListAsync();

        messages.ForEach(m => m.DeliveredAt = DateTime.UtcNow);

        dbContext.Messages.UpdateRange(messages);
        await dbContext.SaveChangesAsync();
        return messages;
    }
}
