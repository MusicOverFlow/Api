using System.Security.Cryptography;

namespace Api.Controllers.AccountControllers;

[ApiController]
[Route("api/accounts")]
public partial class AccountController : ControllerBase
{
    private readonly int MAX_ACCOUNTS_IN_SEARCHES = 20;

    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly DataValidator dataValidator;
    private readonly IConfiguration configuration;
    private readonly LevenshteinDistance stringComparer;
    private readonly ExceptionHandler exceptionHandler;
    private readonly Blob blob;

    public AccountController(
        ModelsContext context,
        Mapper mapper,
        DataValidator dataValidator,
        IConfiguration configuration,
        LevenshteinDistance stringComparer,
        ExceptionHandler exceptionHandler,
        Blob blob)
    {
        this.context = context;
        this.mapper = mapper;
        this.dataValidator = dataValidator;
        this.configuration = configuration;        
        this.stringComparer = stringComparer;
        this.exceptionHandler = exceptionHandler;
        this.blob = blob;
    }

    private void EncryptPassword(string password, out byte[] hash, out byte[] salt)
    {
        using HMACSHA512 hmac = new HMACSHA512();
        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        salt = hmac.Key;
    }
}
