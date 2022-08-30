using Api.Handlers.Kernel;
using Api.Handlers.Queries;
using Api.Models.ExpositionModels.Requests;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet("name"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> ReadName(ReadByNamesRequest request)
    {
        try
        {
            List<Account> accounts = await this.handlers.Get<ReadAccountsByNameQuery>().Handle(request);

            return Ok(accounts
                .Select(a => Mapper.Account_ToResource(a))
                .ToList());
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
