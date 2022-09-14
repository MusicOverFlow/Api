namespace Api.Controllers.CodeControllers;

[ApiController]
[Route("api/execute")]
public partial class CodeController : ControllerBase
{
    private readonly IContainer container;

    public CodeController(IContainer container)
    {
        this.container = container;
    }
}