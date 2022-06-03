namespace Api.Controllers.CodeControllers;

[ApiController]
[Route("api/execute")]
public partial class CodeController : ControllerBase
{
    private readonly IConfiguration configuration;

    public CodeController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
}