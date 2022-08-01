namespace Api.Controllers.GroupControllers;

[ApiController]
[Route("api/groups")]
public partial class GroupController : ControllerBase
{
    private readonly int MAX_GROUPS_IN_SEARCHES = 20;

    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly IConfiguration configuration;
    private readonly LevenshteinDistance stringComparer;
    private readonly ExceptionHandler exceptionHandler;
    private readonly Blob blob;

    public GroupController(
        ModelsContext context,
        Mapper mapper,
        IConfiguration configuration,
        LevenshteinDistance stringComparer,
        ExceptionHandler exceptionHandler,
        Blob blob)
    {
        this.context = context;
        this.mapper = mapper;
        this.configuration = configuration;
        this.stringComparer = stringComparer;
        this.exceptionHandler = exceptionHandler;
        this.blob = blob;
    }
}
