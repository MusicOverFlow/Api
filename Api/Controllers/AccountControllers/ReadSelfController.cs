using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountControllerBase
{
    [HttpGet("self"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<AccountResource>> ReadSelf()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .Include(a => a.OwnedPosts)
            .Include(a => a.OwnedCommentaries)
            .Include(a => a.Groups)
            .Include(a => a.LikedPosts)
            .Include(a => a.LikedCommentaries)
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        return Ok(this.mapper.Account_ToResource_WithGroups_AndPosts(account));
    }
}
