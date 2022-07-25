namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet("pseudonym"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<AccountResource>>> ReadPseudonym(ReadByPseudonym request)
    {
        if (string.IsNullOrWhiteSpace(request.Pseudonym))
        {
            return BadRequest(exceptionHandler.GetError(ErrorType.InvalidPseudonym));
        }

        List<AccountResource> accounts = new List<AccountResource>();

        await this.context.Accounts.ForEachAsync(a =>
        {
            if (accounts.Count >= MAX_ACCOUNTS_IN_SEARCHES)
            {
                return;
            }

            double pseudonymScore = stringComparer.Compare(request.Pseudonym, a.Pseudonym);

            if (pseudonymScore >= 0.6)
            {
                accounts.Add(mapper.Account_ToResource(a));
            }
        });

        return Ok(accounts);
    }
}
