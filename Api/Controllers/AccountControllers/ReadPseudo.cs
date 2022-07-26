namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet("pseudonym"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<AccountResource>>> ReadPseudonym(String pseudonym)
    {
        if (string.IsNullOrWhiteSpace(pseudonym))
        {
            return BadRequest(exceptionHandler.GetError(ErrorType.InvalidPseudonym));
        }

        List<AccountResource> accounts = new List<AccountResource>();

        await this.context.Accounts
            .Include(a => a.Follows)
            .ForEachAsync(a =>
            {
                if (accounts.Count >= MAX_ACCOUNTS_IN_SEARCHES)
                {
                    return;
                }

<<<<<<< Updated upstream
            double pseudonymScore = stringComparer.Compare(pseudonym, a.Pseudonym);
=======
                double pseudonymScore = stringComparer.Compare(request.Pseudonym, a.Pseudonym);
>>>>>>> Stashed changes

                if (pseudonymScore >= 0.6)
                {
                    accounts.Add(mapper.Account_ToResource(a));
                }
            });

        return Ok(accounts);
    }
}
