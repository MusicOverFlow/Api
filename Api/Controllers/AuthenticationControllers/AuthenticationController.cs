namespace Api.Controllers.AuthenticationControllers;

[ApiController]
[Route("api/authenticate")]
public partial class AuthenticationController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly IConfiguration configuration;
    private readonly ExceptionHandler exceptionHandler;

    public AuthenticationController(
        ModelsContext context,
        IConfiguration configuration,
        ExceptionHandler exceptionHandler)
    {
        this.context = context;
        this.configuration = configuration;
        this.exceptionHandler = exceptionHandler;
    }
}
