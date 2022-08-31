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
            List<Account> accounts = await this.handlers.Get<ReadAccountByMailQuery>().Handle(mailAddress);

            return Ok(Mapper.Account_ToResource_WithPosts_AndGroups_AndFollows(accounts[0]));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
