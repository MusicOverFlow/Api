using Api.Handlers.Queries.AccountQueries;
using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet("self"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> ReadSelf()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            Account account = await this.handlers.Get<ReadAccountSelfQuery>().Handle(mailAddress);

            return Ok(Mapper.AccountToResource(account));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
