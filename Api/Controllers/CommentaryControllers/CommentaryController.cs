namespace Api.Controllers.CommentaryControllers;

[ApiController]
[Route("api/commentaries")]
public partial class CommentaryController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly ExceptionHandler exceptionHandler;
    private readonly Blob blob;
    
    public CommentaryController(
        ModelsContext context,
        Mapper mapper,
        ExceptionHandler exceptionHandler,
        Blob blob)
    {
        this.context = context;
        this.mapper = mapper;
        this.exceptionHandler = exceptionHandler;
        this.blob = blob;
    }
}
