using System.Security.Claims;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet("byAccount"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<AccountResource_WithPosts>> ReadByAccount()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Include(a => a.OwnedPosts.OrderByDescending(p => p.CreatedAt))
                .ThenInclude(p => p.Group)
                    .ThenInclude(g => g.Owner)
            .Include(a => a.OwnedCommentaries.OrderByDescending(c => c.Post.CreatedAt))
                .ThenInclude(c => c.Post)
                    .ThenInclude(p => p.Owner)
            .Include(a => a.LikedPosts.OrderByDescending(p => p.CreatedAt))
                .ThenInclude(p => p.Group)
                    .ThenInclude(g => g.Owner)
            .Include(a => a.LikedCommentaries.OrderByDescending(c => c.Post.CreatedAt))
                .ThenInclude(c => c.Post)
                    .ThenInclude(p => p.Group)
                        .ThenInclude(g => g.Owner)
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetException(BadRequestType.AccountNotFound));
        }

        return Ok(this.mapper.Account_ToResource_WithPosts(account));
    }
}
