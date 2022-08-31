using Api.Handlers.Commands.AccountCommands;
using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("follow"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> FollowUnfollow(string mailAddress)
    {
        string callerMail = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            await this.handlers.Get<FollowAccountCommand>().Handle(new FollowDto()
            {
                CallerMail = callerMail,
                TargetMail = mailAddress,
            });

            return Ok();
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
