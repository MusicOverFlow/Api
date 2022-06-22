namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet("name"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<AccountResource>>> ReadName(ReadByNames request)
    {
        if (string.IsNullOrWhiteSpace(request.Firstname) && string.IsNullOrWhiteSpace(request.Lastname))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.InvalidName));
        }

        List<AccountResource> accounts = new List<AccountResource>();

        await this.context.Accounts.ForEachAsync(a =>
        {
            if (accounts.Count >= this.MAX_ACCOUNTS_IN_SEARCHES)
            {
                return;
            }

            double lastnameScore = this.stringComparer.Compare(request.Lastname, a.Lastname);

            if (lastnameScore >= 0.6)
            {
                accounts.Add(this.mapper.Account_ToResource(a));
            }
            else if (!string.IsNullOrWhiteSpace(request.Firstname))
            {
                double firstnameScore = this.stringComparer.Compare(request.Firstname, a.Firstname);

                if ((lastnameScore + firstnameScore) >= 1.1)
                {
                    accounts.Add(this.mapper.Account_ToResource(a));
                }
            }
        });

        return Ok(accounts);
    }
}
