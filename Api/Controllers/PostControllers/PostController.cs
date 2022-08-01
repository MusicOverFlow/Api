using Azure.Storage.Blobs;

namespace Api.Controllers.PostControllers;

[ApiController]
[Route("api/posts")]
public partial class PostController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly IConfiguration configuration;
    private readonly ExceptionHandler exceptionHandler;
    private readonly Blob blob;

    public PostController(
        ModelsContext context,
        Mapper mapper,
        IConfiguration configuration,
        ExceptionHandler exceptionHandler,
        Blob blob)
    {
        this.context = context;
        this.mapper = mapper;
        this.configuration = configuration;
        this.exceptionHandler = exceptionHandler;
        this.blob = blob;
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
