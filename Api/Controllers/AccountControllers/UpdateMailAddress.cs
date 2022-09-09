using Api.Handlers.Commands.AccountCommands;
using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("mail"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> UpdateMailAddress(string mailAddress)
    {
        string actualMailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            Account account = await this.handlers.Get<UpdateAccountMailAddressCommand>().Handle(new UpdateMailDto()
            {
                MailAddress = actualMailAddress,
                NewMailAddress = mailAddress,
            });

            return Ok(Mapper.AccountToResource(account));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
