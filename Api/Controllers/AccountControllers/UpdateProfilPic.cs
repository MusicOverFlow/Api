using Api.Handlers.Commands.AccountCommands;
using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("profilpic"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> UpdateProfilPic()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            Account account = await this.handlers.Get<UpdateAccountProfilPicCommand>().Handle(new UpdateProfilPicDto()
            {
                MailAddress = mailAddress,
                ProfilPic = this.Request.Form.Files.FirstOrDefault(),
            });

            return Ok(Mapper.AccountToResource(account));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
