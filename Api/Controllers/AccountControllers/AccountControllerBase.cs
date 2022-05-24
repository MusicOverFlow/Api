namespace Api.Controllers.AccountControllers;

[ApiController]
[Route("api/accounts")]
public partial class AccountControllerBase : ControllerBase
{
    protected readonly int MAX_ACCOUNTS_IN_SEARCHES = 20;

    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly DataValidator dataValidator;
    private readonly Utilitaries.StringComparer stringComparer;

    public AccountControllerBase(
        ModelsContext context,
        Mapper mapper,
        DataValidator dataValidator,
        Utilitaries.StringComparer stringComparer)
    {
        this.context = context;
        this.mapper = mapper;
        this.dataValidator = dataValidator;
        this.stringComparer = stringComparer;
    }
}
