namespace Api.Controllers.AccountControllers;

[ApiController]
[Route("api/accounts")]
public partial class AccountController : ControllerBase
{
    private readonly HandlersContainer handlers;

    public AccountController(HandlersContainer handlers)
    {
        this.handlers = handlers;
    }
}
