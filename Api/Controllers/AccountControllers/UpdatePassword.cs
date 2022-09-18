using Api.Handlers.Commands.AccountCommands;
using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("password"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> UpdatePassword(string password)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            Account account = await this.handlers.Get<UpdateAccountPasswordCommand>().Handle(new UpdatePasswordDto()
            {
                MailAddress = mailAddress,
                NewPassword = password,
            });

            return Ok(Mapper.AccountToResource(account));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
