using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet("groupPosts")]
    public async Task<ActionResult<List<PostResource_WithCommentaries_AndLikes>>> ReadAccountGroupPosts()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Include(a => a.Groups)
                .ThenInclude(g => g.Posts.OrderByDescending(g => g.CreatedAt))
                    .ThenInclude(p => p.Commentaries)
            .Include(a => a.Groups)
                .ThenInclude(g => g.Posts)
                    .ThenInclude(p => p.Likes)
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        List<Post> posts = account.Groups
            .SelectMany(g => g.Posts)
            .ToList();
        
        List<PostResource_WithCommentaries_AndLikes> postsResult = new List<PostResource_WithCommentaries_AndLikes>();
        posts.ForEach(p => postsResult.Add(this.mapper.Post_ToResource_WithCommentaries_AndLikes(p)));

        return Ok(postsResult);
    }
}
