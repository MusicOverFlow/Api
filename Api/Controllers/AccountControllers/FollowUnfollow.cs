using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("follow"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> FollowUnfollow(string mailAddress)
    {
        string callerMailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.AccountNotFound);
        }

        Account callerAccount = await this.context.Accounts
            .Include(a => a.Follows)
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(callerMailAddress));

        if (callerAccount.Equals(account))
        {
            return BadRequest(this.exceptionHandler.SelfFollow);
        }

        if (!callerAccount.Follows.Remove(account))
        {
            callerAccount.Follows.Add(account);
        }

        await this.context.SaveChangesAsync();

        return Ok();
    }
}
