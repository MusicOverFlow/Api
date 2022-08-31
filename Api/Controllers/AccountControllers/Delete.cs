using Api.Handlers.Commands.AccountCommands;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpDelete, AuthorizeEnum(Role.Admin)]
    public async Task<ActionResult> Delete(string mailAddress)
    {
        try
        {
            await this.handlers.Get<DeleteAccountCommand>().Handle(mailAddress);

            return Ok();
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
