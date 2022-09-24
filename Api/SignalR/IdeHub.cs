using Microsoft.AspNetCore.SignalR;

namespace Api.SignalR;

public class IdeHub : Hub
{
    public async override Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
    
    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnConnectedAsync();
    }

    public async Task<string> JoinGroup(string groupId = null)
    {
        if (string.IsNullOrWhiteSpace(groupId))
        {
            string newGroupId = Guid.NewGuid().ToString();
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, newGroupId);
            return newGroupId;
        }
        
        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupId);
        return "JOINED";
    }

    public async Task<string> UpdateContent(string groupId, string content)
    {
        await this.Clients.GroupExcept(groupId, this.Context.ConnectionId).SendAsync("ReceiveContent", content);
        return "UPDATED";
    }
}
