namespace Api.Controllers.GroupControllers;

[ApiController]
[Route("api/groups")]
public partial class GroupController : ControllerBase
{
    private readonly int MAX_GROUPS_IN_SEARCHES = 20;

    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly Utilitaries.StringComparer stringComparer;

    public GroupController(
        ModelsContext context,
        Mapper mapper,
        Utilitaries.StringComparer stringComparer)
    {
        this.context = context;
        this.mapper = mapper;
        this.stringComparer = stringComparer;
    }
}
