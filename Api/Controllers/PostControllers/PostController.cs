using System.Security.Claims;

namespace Api.Controllers.PostControllers;

[ApiController]
[Route("api/posts")]
public partial class PostController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly Mapper mapper;

    public PostController(
        ModelsContext context,
        Mapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
}
