using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("like"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> LikeDislikePost(Guid? id = null)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Include(a => a.LikedPosts)
            .Include(a => a.LikedCommentaries)
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetException(BadRequestType.AccountNotFound));
        }

        Post post = await this.context.Posts
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.Id.Equals(id));

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
                .Include(c => c.Likes)
                .FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (commentary == null)
            {
                return NotFound(this.exceptionHandler.GetException(BadRequestType.PostOrCommentaryNotFound));
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
