using Api.Handlers.Queries.AccountQueries;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet("pseudonym"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> ReadPseudonym(string pseudonym)
    {
        try
        {
            List<Account> accounts = await this.handlers.Get<ReadAccountByPseudonymQuery>().Handle(pseudonym);

            return Ok(accounts
                .Select(a => Mapper.AccountToResource(a))
                .ToList());
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
