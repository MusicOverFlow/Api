using System.Security.Claims;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet("byAccount")]
    public async Task<ActionResult<AccountResource_WithPosts>> ReadByAccount()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Include(a => a.OwnedPosts)
                .ThenInclude(p => p.Group)
                    .ThenInclude(g => g.Owner)
            .Include(a => a.LikedPosts)
                .ThenInclude(p => p.Group)
                    .ThenInclude(g => g.Owner)
            .Include(a => a.LikedCommentaries)
                .ThenInclude(c => c.Post)
                    .ThenInclude(p => p.Group)
                        .ThenInclude(g => g.Owner)
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(new { errorMessage = "Account not found" });
        }

        return Ok(this.mapper.Account_ToResource_WithPosts(account));
    }
}
