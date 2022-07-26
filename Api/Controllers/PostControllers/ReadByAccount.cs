using System.Security.Claims;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet("byAccount"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<PostResource>> ReadByAccount()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        List<PostResource> posts = account.OwnedPosts
            .Select(p => this.mapper.Post_ToResource(p))
            .ToList();

        return Ok(posts);
    }
}
