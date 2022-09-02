namespace Api.Controllers.PostControllers;

[ApiController]
[Route("api/posts")]
public partial class PostController : ControllerBase
{
    private readonly HandlersContainer handlers;

    public PostController(HandlersContainer handlers)
    {
        this.handlers = handlers;
    }
}
