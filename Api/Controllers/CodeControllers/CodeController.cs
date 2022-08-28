namespace Api.Controllers.CodeControllers;

[ApiController]
[Route("api/execute")]
public partial class CodeController : ControllerBase
{
    private readonly Blob blob;

    public CodeController(Blob blob)
    {
        this.blob = blob;
    }
}