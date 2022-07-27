namespace Api.Controllers.PostControllers;

[ApiController]
[Route("api/posts")]
public partial class PostController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly ExceptionHandler exceptionHandler;

    public PostController(
        ModelsContext context,
        Mapper mapper,
        ExceptionHandler exceptionHandler)
    {
        this.context = context;
        this.mapper = mapper;
        this.exceptionHandler = exceptionHandler;
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
