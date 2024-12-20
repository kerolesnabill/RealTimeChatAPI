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

            await Clients.Caller.SendAsync("Message", jsonMsg);

            if (userConnections.TryGetValue(recipient.Id, out var connectionId))
                await Clients.Client(connectionId).SendAsync("Message", jsonMsg);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }
}
