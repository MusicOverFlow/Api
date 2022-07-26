namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<AccountResource>>> Read(string mailAddress = null)
    {
        IQueryable<Account> query = this.context.Accounts
            .Include(a => a.Follows);

        if (!string.IsNullOrWhiteSpace(mailAddress))
        {
            query = query.Where(a => a.MailAddress.Equals(mailAddress));
        }

        List<AccountResource_WithPosts_AndGroups_AndFollows> accounts = new List<AccountResource_WithPosts_AndGroups_AndFollows>();

        await query.ForEachAsync(a => accounts.Add(this.mapper.Account_ToResource_WithPosts_AndGroups_AndFollows(a)));

        if (!string.IsNullOrWhiteSpace(mailAddress) && accounts.Count == 0)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        return Ok(accounts);
    }
}
