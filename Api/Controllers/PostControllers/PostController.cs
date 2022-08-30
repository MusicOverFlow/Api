using Api.Handlers;
using Api.Models.ExpositionModels.Resources;

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

    private bool Contains(List<PostResource> posts, Guid id)
    {
        bool result = false;
        posts.ForEach(p =>
        {
            if (p.Id.Equals(id)) result = true;
        });
        return result;
    }
}
