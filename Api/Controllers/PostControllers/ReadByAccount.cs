using Api.Models.ExpositionModels.Resources;
using System.Security.Claims;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet("byAccount"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<PostResource>>> ReadByAccount()
    {
        /*
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        List<PostResource> posts = new List<PostResource>();

        account.OwnedPosts.ToList().ForEach(p =>
        {
            if (!this.Contains(posts, p.Id)) posts.Add(this.mapper.Post_ToResource(p));
        });
        account.OwnedCommentaries.ToList().ForEach(c =>
        {
            if (!this.Contains(posts, c.Post.Id)) posts.Add(this.mapper.Post_ToResource(c.Post));
        });

        return Ok(posts.OrderByDescending(p => p.CreatedAt).ToList());
        */
        return Ok();
    }
}
