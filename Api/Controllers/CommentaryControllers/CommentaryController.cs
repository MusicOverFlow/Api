using Api.Handlers;

namespace Api.Controllers.CommentaryControllers;

[ApiController]
[Route("api/commentaries")]
public partial class CommentaryController : ControllerBase
{
    private readonly HandlersContainer handlers;

    public CommentaryController(HandlersContainer handlers)
    {
        this.handlers = handlers;
    }
}
