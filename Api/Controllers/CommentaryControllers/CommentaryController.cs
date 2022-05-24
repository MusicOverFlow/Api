namespace Api.Controllers.CommentaryControllers;

[ApiController]
[Route("api/commentaries")]
public partial class CommentaryController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly Mapper mapper;

    public CommentaryController(
        ModelsContext context,
        Mapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
}
