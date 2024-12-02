using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs;

[Authorize]
public class ChatHub() : Hub
{
    private static readonly Dictionary<string, string> userConnections = [];

    public override async Task OnConnectedAsync()
    {
        var user = Context.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            Context.Abort();
            return;
        }

        var username = user.Identity.Name;
        userConnections.TryAdd(username!, Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = userConnections
               .FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        if(username != null)
            userConnections.Remove(username);
        await base.OnDisconnectedAsync(exception);
    }

    public Task SendMessageToUser(string username, string message)
    {
        if (userConnections.TryGetValue(username.ToLower(), out var connectionId))
            return Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        
        return Task.CompletedTask;
    }
}
