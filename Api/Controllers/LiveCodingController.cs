using Api.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace Api.Controllers;

[ApiController]
[Route("api/ide")]
public class LiveCodingController : ControllerBase
{
    private readonly IHubContext<IdeHub> hubContext;
    private readonly HubConnection hubConnection;

    public LiveCodingController(IHubContext<IdeHub> hubContext)
    {
        this.hubContext = hubContext;
        this.hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:7143/livecoding")
            .Build();
    }

    [HttpPost("createroom")]
    public async Task<ActionResult> Create()
    {
        try
        {
            await this.hubConnection.StartAsync();
            Console.WriteLine(hubConnection.ConnectionId);
            
            string groupId = Guid.NewGuid().ToString();
            await this.hubContext.Groups.AddToGroupAsync(this.hubConnection.ConnectionId, groupId);
            
            return Ok(new
            {
                groupId = groupId,
            });
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("joinroom")]
    public async Task<ActionResult> Join(string id)
    {
        try
        {
            await this.hubConnection.StartAsync();
            Console.WriteLine(hubConnection.ConnectionId);
            
            await this.hubContext.Groups.AddToGroupAsync(this.hubConnection.ConnectionId, id);
            
            return Ok();
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("updateroom")]
    public async Task<ActionResult> Update(string id)
    {
        try
        {
            string script = string.Empty;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                script = await reader.ReadToEndAsync();
            }

            await this.hubConnection.StartAsync();
            Console.WriteLine(hubConnection.ConnectionId);
            
            await this.hubContext.Clients.GroupExcept(id, this.hubConnection.ConnectionId).SendAsync("UpdateContent", script);
            
            return Ok();
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
