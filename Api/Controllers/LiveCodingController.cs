using Api.SignalR;

namespace Api.Controllers;

[ApiController]
[Route("api/ide")]
public class LiveCodingController : ControllerBase
{
    private readonly IdeHub hub;

    public LiveCodingController(IdeHub hub)
    {
        this.hub = hub;
    }

    [HttpPost("createroom")]
    public async Task<ActionResult> Create()
    {
        try
        {
            string groupId = await this.hub.JoinGroup();
            
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
            await this.hub.JoinGroup(id);

            return Ok();
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
