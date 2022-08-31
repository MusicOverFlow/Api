using Api.Handlers.Commands.AccountCommands;
using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("profil"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> UpdateProfil(UpdateProfilRequest request)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            await this.handlers.Get<UpdateAccountProfilCommand>().Handle(new UpdateProfilDto()
            {
                MailAddress = mailAddress,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Pseudonym = request.Pseudonym,
            });

            return Ok();
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
