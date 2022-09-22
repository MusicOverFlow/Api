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
}
