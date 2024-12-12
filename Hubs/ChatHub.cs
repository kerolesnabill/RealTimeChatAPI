using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Exceptions;
using RealTimeChatAPI.Models;
using System.Text.Json;

namespace RealTimeChatAPI.Hubs;

[Authorize]
public class ChatHub(
        IUsersRepository usersRepository,
        IChatsRepository chatsRepository,
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

    public async Task CreateChat(string userId)
    {
        try
        {
            var isValidId = Guid.TryParse(userId, out var userGuid);
            if (!isValidId) throw new Exception("Invalid user id");

            var currentUserId = Guid.Parse(Context.UserIdentifier!);
            var currentUser = await usersRepository.GetByIdAsync(currentUserId)
                ?? throw new NotFoundException(nameof(User), Context.UserIdentifier!);

            var user = await usersRepository.GetByIdAsync(userGuid)
                ?? throw new NotFoundException(nameof(User), userId);

            var chat = await chatsRepository.GetByUsersAsync(currentUserId, user.Id);
            if (chat != null) throw new Exception("Chat already exists");

            chat = await chatsRepository.CreateAsync(new Chat
            {
                Name = $"{Context?.User?.Identity?.Name}-{user.Username}",
                ChatUsers = [new() { UserId = currentUserId }, new() { UserId = user.Id }]
            });

            var chatDto = mapper.Map<ChatDto>(chat);
            chatDto.Name = user.Name;
            chatDto.Image = user.Image;
            await Clients.Caller.SendAsync("Chat", JsonSerializer.Serialize(chatDto));

            if (userConnections.TryGetValue(user.Id, out var connectionId))
            {
                chatDto.Name = currentUser.Name;
                chatDto.Image = currentUser.Image;
                await Clients.Client(connectionId).SendAsync("Chat", JsonSerializer.Serialize(chatDto));
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

    public async Task SendMessage(string chatId, string message)
    {
        try
        {
            var isValidId = Guid.TryParse(chatId, out var chatGuid);
            if (!isValidId) throw new Exception("Invalid chat Id");

            if (!string.IsNullOrWhiteSpace(message))
                throw new Exception("Message is empty");

            var chat = await chatsRepository.GetByIdAsync(chatGuid)
                ?? throw new NotFoundException(nameof(Chat), chatId);

            var currentUserId = Guid.Parse(Context.UserIdentifier!);
            var chatUser = chat.ChatUsers.FirstOrDefault(cu => cu.UserId == currentUserId)
                ?? throw new Exception("User is not a member of this chat");

            var msg = await messagesRepository.CreateAsync(new()
            {
                ChatId = chat.Id,
                SenderId = currentUserId,
                Content = message
            });

            var jsonMsg = JsonSerializer.Serialize(mapper.Map<MessageDto>(message));

            if (chat.IsGroupChat == true)
                await Clients.Group(chat.Id.ToString()).SendAsync("Message", jsonMsg);
            else
            {
                await Clients.Caller.SendAsync("Message", jsonMsg);
                var id = chat.ChatUsers.FirstOrDefault(cu => cu.UserId != currentUserId)!.UserId;

                if(userConnections.TryGetValue(id, out var connectionId))
                    await Clients.Client(connectionId).SendAsync("Message", jsonMsg);
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
        }
    }

}
