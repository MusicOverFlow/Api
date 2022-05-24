namespace Api.Controllers.AuthenticationControllers;

[ApiController]
[Route("api/authenticate")]
public partial class AuthenticationController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly IConfiguration configuration;

    public AuthenticationController(ModelsContext context, IConfiguration configuration)
    {
        this.context = context;
        this.configuration = configuration;
    }
}
