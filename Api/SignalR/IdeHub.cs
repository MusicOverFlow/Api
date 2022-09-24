using Microsoft.AspNetCore.SignalR;

namespace Api.SignalR;

public class IdeHub : Hub
{
    private readonly Dictionary<string, string> groupsContents = new Dictionary<string, string>();
    
    public async Task<string> JoinGroup(string groupId = null)
    {
        if (string.IsNullOrWhiteSpace(groupId))
        {
            string newGroupId = Guid.NewGuid().ToString();
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, newGroupId);
            this.groupsContents.Add(newGroupId, string.Empty);
            return newGroupId;
        }

        if (this.groupsContents.TryGetValue(groupId, out string content))
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupId);
            return content;
        }
        
        return $"Error : group {groupId} does not exists";
    }

    public async Task UpdateContent(string groupId, string content)
    {
        await this.Clients.GroupExcept(groupId, this.Context.ConnectionId).SendAsync("ReceiveContent", content);
        this.groupsContents[groupId] = content;
    }
}
