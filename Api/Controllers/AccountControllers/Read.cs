namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<AccountResource_WithPosts_AndGroups>>> Read(string mailAddress = null)
    {
        IQueryable<Account> query = this.context.Accounts
            .Include(a => a.OwnedPosts)
            .Include(a => a.LikedPosts)
            .Include(a => a.LikedCommentaries)
            .Include(a => a.Groups)
            .Include(a => a.Follows);

        if (!string.IsNullOrWhiteSpace(mailAddress))
        {
            query = query.Where(a => a.MailAddress.Equals(mailAddress));
        }

        List<AccountResource_WithPosts_AndGroups> accounts = new List<AccountResource_WithPosts_AndGroups>();

        await query.ForEachAsync(a => accounts.Add(this.mapper.Account_ToResource_WithPosts_AndGroups(a)));

        if (!string.IsNullOrWhiteSpace(mailAddress) && accounts.Count == 0)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        return Ok(accounts);
    }
}
