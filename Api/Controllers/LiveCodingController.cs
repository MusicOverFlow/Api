using Microsoft.AspNetCore.SignalR.Client;

namespace Api.Controllers;

[ApiController]
[Route("api/ide")]
public class LiveCodingController : ControllerBase
{
    [HttpPost("room")]
    public async Task<ActionResult> CreateLiveCodingRoom()
    {
        try
        {
            HubConnection hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7143/livecoding")
                .Build();

            hubConnection.On<string>("UpdateContent", (message) =>
            {
                Console.WriteLine(message);
            });

            await hubConnection.StartAsync();
            
            return Ok(new { connectionId = hubConnection.ConnectionId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}
