using Api.Handlers.Commands.AccountCommands;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("role"), AuthorizeEnum(Role.Admin)]
    public async Task<ActionResult> UpdateRole(string mailAddress, string role)
    {
        try
        {
            await this.handlers.Get<UpdateAccountRoleCommand>().Handle(new UpdateAccountRoleDto()
            {
                MailAddress = mailAddress,
                Role = role,
            });

            return Ok();
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
