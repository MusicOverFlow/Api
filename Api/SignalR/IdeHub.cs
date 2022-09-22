using Microsoft.AspNetCore.SignalR;

namespace Api.SignalR;

public class IdeHub : Hub
{
    public async override Task OnConnectedAsync()
    {
        Console.WriteLine($"Hé y'a quelqu'un la ! {this.Context.ConnectionId}");
        await base.OnConnectedAsync();
    }
    
    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"o no il est parti parce que {exception.Message ?? "... on sait pas chelou si tu veux mon avis"}");
        await base.OnConnectedAsync();
    }

    public async Task<string> JoinGroup(string groupId = null)
    {
        if (string.IsNullOrWhiteSpace(groupId))
        {
            string newGroupId = Guid.NewGuid().ToString();
            Console.WriteLine($"Ca créé un groupe pour {this.Context.ConnectionId} ({newGroupId})");
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, newGroupId);
            return newGroupId;
        }

        Console.WriteLine($"Ca rejoint le groupe {groupId} pour {this.Context.ConnectionId}");
        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupId);
        return null;
    }

    public async Task UpdateContent(string groupId, string content)
    {
        Console.WriteLine($"Ca update le contenu pour le groupe {groupId}\nContenu :\n{content}");
        await this.Clients.GroupExcept(groupId, this.Context.ConnectionId).SendAsync("UpdateContent", content);
    }
}
