using Api.Handlers;

namespace Api.Controllers.AuthenticationControllers;

[ApiController]
[Route("api/authenticate")]
public partial class AuthenticationController : ControllerBase
{
    private readonly HandlersContainer handlers;
    private readonly IConfiguration configuration;

    public AuthenticationController(
        HandlersContainer handlers,
        IConfiguration configuration)
    {
        this.handlers = handlers;
        this.configuration = configuration;
    }
}
