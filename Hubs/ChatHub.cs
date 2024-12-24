using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Exceptions;
using RealTimeChatAPI.Models;
using System.Text.Json;
using RealTimeChatAPI.Data.Repositories;

namespace RealTimeChatAPI.Hubs;

[Authorize]
public class ChatHub(
        IUsersRepository usersRepository,
        IMessagesRepository messagesRepository,
        IMapper mapper) : Hub
{
    private static readonly Dictionary<Guid, string> userConnections = [];

    public override async Task OnConnectedAsync()
    {
        if (Context.User?.Identity?.IsAuthenticated != true)
        {
            Context.Abort();
            return;
        }

        var userId = Guid.Parse(Context.UserIdentifier!);
        userConnections.TryAdd(userId, Context.ConnectionId);
        await DeliveredAllMessages();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Guid.Parse(Context.UserIdentifier!);
        userConnections.Remove(userId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string userId, string message)
    {
        try
        {
            var isValidId = Guid.TryParse(userId, out var userGuid);
            if (!isValidId) throw new Exception("Invalid user Id");

            if (string.IsNullOrWhiteSpace(message))
                throw new Exception("Message is empty");

            var recipient = await usersRepository.GetByIdAsync(userGuid)
                ?? throw new NotFoundException(nameof(User), userId);

            var senderId = Guid.Parse(Context.UserIdentifier!);

            var msg = await messagesRepository.AddAsync(new()
            {
                SenderId = senderId,
                RecipientId = recipient.Id,
                Content = message
            });

            var jsonMsg = JsonSerializer.Serialize(mapper.Map<MessageDto>(msg));

            await Clients.Caller.SendAsync("SendMessage", jsonMsg);

            if (userConnections.TryGetValue(recipient.Id, out var connectionId))
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", jsonMsg);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

    public async Task DeliveredMessage(string messageId)
    {
        try
        {
            var isValidId = Guid.TryParse(messageId, out var msgId);
            if (!isValidId) throw new Exception("Invalid message Id");

            var message = await messagesRepository.GetByIdAsync(msgId)
                ?? throw new NotFoundException(nameof(Message), messageId);

            var userId = Guid.Parse(Context.UserIdentifier!);
            if (message.RecipientId != userId) return;

            if (message.DeliveredAt == null)
                message.DeliveredAt = DateTime.UtcNow;

            var msg = await messagesRepository.UpdateAsync(message);
            var jsonMsg = "[" + JsonSerializer.Serialize(mapper.Map<MessageDto>(msg)) + "]";
            

            if (userConnections.TryGetValue(message.SenderId, out var connectionId))
                await Clients.Client(connectionId).SendAsync("MessageStatus", jsonMsg);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

    public async Task ReadMessages(string userChatId)
    {
        try
        {
            var isValidId = Guid.TryParse(userChatId, out var userChatGuid);
            if (!isValidId) throw new Exception("Invalid user chat Id");

            var user = await usersRepository.GetByIdAsync(userChatGuid)
                ?? throw new NotFoundException(nameof(User), userChatId);

            var userId = Guid.Parse(Context.UserIdentifier!);
            var messages = await messagesRepository.ReadUnreadMessagesAsync(userId, userChatGuid);

            var jsonMessages = JsonSerializer.Serialize(mapper.Map<IEnumerable<MessageDto>>(messages));

            if (userConnections.TryGetValue(user.Id, out var connectionId))
                await Clients.Client(connectionId).SendAsync("MessageStatus", jsonMessages);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

    private async Task DeliveredAllMessages()
    {
        var userId = Guid.Parse(Context.UserIdentifier!);
        var messages = await messagesRepository.DeliveredAllMessagesAsync(userId);

        var groups = messages.GroupBy(m => m.SenderId);
        foreach (var group in groups)
        {
            var jsonMessages = JsonSerializer.Serialize(mapper.Map<IEnumerable<MessageDto>>(group));
            if (userConnections.TryGetValue(group.First().SenderId, out var connectionId))
                await Clients.Client(connectionId).SendAsync("MessageStatus", jsonMessages);
        }
    }
}
