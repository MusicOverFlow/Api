using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace Api.SignalR;

public class IdeHub : Hub
{
    public void BroadcastMessage(string name, string message)
    {
        Clients.All.SendAsync("broadcastMessage", name, message);
    }

    public void Echo(string name, string message)
    {
        Clients.Client(Context.ConnectionId).SendAsync("echo", name, message + " (echo from server)");
    }
    
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("OnConnected as ");
        await this.SendNotification("Nouvel utilisateur connecté");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("OnDisconnected");
        await this.SendNotification("Utilisateur déconnecté");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task UpdateContent(string content)
    {
        Console.WriteLine("UpdateContent with " + content);
        await this.Clients.All.SendAsync("content", content);
    }

    private async Task SendNotification(string message)
    {
        await this.Clients.All.SendAsync("notification", message);
    }
}
