using System.Security.Claims;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpPost, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<PostResource>> Create(CreatePost request, Guid? groupId)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(new { message = "Title or content is empty" });
        }

        Group group = null;
        if (groupId != null)
        {
            group = await this.context.Groups
                .FirstOrDefaultAsync(g => g.Id.Equals(groupId));

            if (group == null)
            {
                return NotFound(new { message = "Group not found" });
            }
        }

        Post post = new Post()
        {
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.Now,

            Owner = account,
            Group = group,
        };

        this.context.Posts.Add(post);

        await this.context.SaveChangesAsync();

        return Created(nameof(Create), post);
    }
}
