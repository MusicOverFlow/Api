using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountControllerBase
{
    [HttpPut("like"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> LikePost(Guid? id = null)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .Include(a => a.LikedPosts)
            .Include(a => a.LikedCommentaries)
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        Post post = await this.context.Posts
            .Where(p => p.Id.Equals(id))
            .Include(p => p.Likes)
            .FirstOrDefaultAsync();

        if (post != null)
        {
            if (post.Likes.Contains(account))
            {
                post.Likes.Remove(account);
                account.LikedPosts.Remove(post);
            }
            else
            {
                post.Likes.Add(account);
                account.LikedPosts.Add(post);
            }
            post.LikesCount = post.Likes.Count;
        }
        else
        {
            Commentary commentary = await this.context.Commentaries
                .Where(c => c.Id.Equals(id))
                .Include(c => c.Likes)
                .FirstOrDefaultAsync();

            if (commentary == null)
            {
                return NotFound(new { message = "Post or commentary not found" });
            }

            if (commentary.Likes.Contains(account))
            {
                commentary.Likes.Remove(account);
                account.LikedCommentaries.Remove(commentary);
            }
            else
            {
                commentary.Likes.Add(account);
                account.LikedCommentaries.Add(commentary);
            }
            commentary.LikesCount = post.Likes.Count;
        }

        await this.context.SaveChangesAsync();

        return Ok();
    }
}
