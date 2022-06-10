using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet("self"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<AccountResource>> ReadSelf()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Include(a => a.OwnedPosts)
            .Include(a => a.LikedPosts)
            .Include(a => a.LikedCommentaries)
            .Include(a => a.Groups)
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetException(BadRequestType.AccountNotFound));
        }

        return Ok(this.mapper.Account_ToResource_WithPosts_AndGroups(account));
    }
}
