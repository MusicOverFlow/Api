using System.Security.Claims;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet("homePage"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<PostResource_WithCommentaries_AndLikes>> ReadHomePage()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        // TODO: OOF, obligé y'a plus simple, à creuser
        Account account = await this.context.Accounts
            .Include(a => a.OwnedPosts)
                .ThenInclude(p => p.Group)
                    .ThenInclude(g => g.Owner)
            .Include(a => a.OwnedCommentaries)
                .ThenInclude(c => c.Post)
                    .ThenInclude(p => p.Owner)
            .Include(a => a.LikedPosts)
                .ThenInclude(p => p.Group)
                    .ThenInclude(g => g.Owner)
            .Include(a => a.LikedCommentaries)
                .ThenInclude(c => c.Post)
                    .ThenInclude(p => p.Group)
                        .ThenInclude(g => g.Owner)
            .Include(a => a.Follows)
                .ThenInclude(f => f.OwnedPosts)
                    .ThenInclude(p => p.Commentaries)
                        .ThenInclude(c => c.Likes)
            .Include(a => a.Follows)
                .ThenInclude(f => f.OwnedPosts)
                    .ThenInclude(p => p.Commentaries)
            .Include(a => a.Follows)
                .ThenInclude(f => f.OwnedCommentaries)
                    .ThenInclude(p => p.Likes)
            .Include(a => a.Follows)
                .ThenInclude(f => f.OwnedCommentaries)
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        List<PostResource_WithCommentaries_AndLikes> posts = new List<PostResource_WithCommentaries_AndLikes>();

        account.OwnedPosts.ToList().ForEach(p =>
        {
            posts.Add(this.mapper.Post_ToResource_WithCommentaries_AndLikes(p));
        });
        account.OwnedCommentaries.ToList().ForEach(c =>
        {
            posts.Add(this.mapper.Post_ToResource_WithCommentaries_AndLikes(c.Post));
        });
        account.Follows.ToList().ForEach(f =>
        {
            f.OwnedPosts.ToList().ForEach(p =>
            {
                posts.Add(this.mapper.Post_ToResource_WithCommentaries_AndLikes(p));
            });
            f.OwnedCommentaries.ToList().ForEach(c =>
            {
                posts.Add(this.mapper.Post_ToResource_WithCommentaries_AndLikes(c.Post));
            });
        });

        return Ok(posts.Take(20).OrderByDescending(p => p.CreatedAt).ToList());
    }
}
