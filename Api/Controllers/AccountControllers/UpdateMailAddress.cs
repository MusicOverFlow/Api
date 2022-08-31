using Api.Handlers.Commands.AccountCommands;
using Api.Handlers.Dtos;
using Api.Handlers.Kernel;
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
            await this.handlers.Get<UpdateAccountMailAddressCommand>().Handle(new UpdateMailDto()
            {
                MailAddress = actualMailAddress,
                NewMailAddress = mailAddress,
            });
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
        
        return Ok();
    }
}
