using System.Security.Claims;

namespace Api.Controllers;

[Route("api/commentaries")]
[ApiController]
public class CommentariesController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly Mapper mapper;

    public CommentariesController(
        ModelsContext context,
        Mapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<CommentaryResource>> Create(CreateCommentary request, Guid? postId)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { errorMessage = "Account not found" });
        }

        Post post = await this.context.Posts
            .FirstOrDefaultAsync(p => p.Id.Equals(postId));

        if (post == null)
        {
            return NotFound(new { errorMessage = "Post not found" });
        }

        Commentary commentary = new Commentary
        {
            Content = request.Content,
            CreatedAt = DateTime.Now,
            Owner = account,
            Post = post,
        };

        this.context.Commentaries.Add(commentary);
        post.Commentaries.Add(commentary);

        await this.context.SaveChangesAsync();

        return Created(nameof(Create), this.mapper.Post_ToResource(post));
    }
}
