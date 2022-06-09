namespace Api.Controllers.CommentaryControllers;

[ApiController]
[Route("api/commentaries")]
public partial class CommentaryController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly ExceptionHandler exceptionHandler;

    public CommentaryController(
        ModelsContext context,
        Mapper mapper,
        ExceptionHandler exceptionHandler)
    {
        this.context = context;
        this.mapper = mapper;
        this.exceptionHandler = exceptionHandler;
    }
}
