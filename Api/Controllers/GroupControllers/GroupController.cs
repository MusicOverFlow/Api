using Api.Handlers;

namespace Api.Controllers.GroupControllers;

[ApiController]
[Route("api/groups")]
public partial class GroupController : ControllerBase
{
    private readonly HandlersContainer handlers;

    public GroupController(HandlersContainer handlers)
    {
        this.handlers = handlers;
    }
}
