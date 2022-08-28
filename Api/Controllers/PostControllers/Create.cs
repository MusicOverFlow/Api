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
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.PostTitleOrContentEmpty));
        }

        if (!string.IsNullOrWhiteSpace(request.ScriptLanguage))
        {
            request.ScriptLanguage = request.ScriptLanguage.ToLower();
            if (!this.IsScriptLanguageSupported(request.ScriptLanguage))
            {
                return BadRequest(new { error = $"Unsupported language {request.ScriptLanguage}" });
            }
        }
        else
        {
            request.ScriptLanguage = null;
        }

        Group group = null;
        if (groupId != null)
        {
            group = await this.context.Groups
                .FirstOrDefaultAsync(g => g.Id.Equals(groupId));

            if (group == null)
            {
                return NotFound(this.exceptionHandler.GetError(ErrorType.GroupNotFound));
            }
        }

        Post post = new Post()
        {
            Title = request.Title ?? "No title",
            Content = request.Content,
            CreatedAt = DateTime.Now,

            ScriptLanguage = request.ScriptLanguage,
            
            Owner = account,
            Group = group,

            MusicUrl = null,
        };

        this.context.Posts.Add(post);
        post.ScriptUrl = request.Script != null ? this.blob.GetPostScriptUrl(request.Script, post.Id).Result : null;
        await this.context.SaveChangesAsync();

        return Created(nameof(Create), this.mapper.Post_ToResource(post));
    }

    private bool IsScriptLanguageSupported(string language)
    {
        foreach (int lInt in Enum.GetValues(typeof(Language)))
        {
            Language lEnum = (Language)lInt;
            string lString = lEnum.ToString().ToLower();

            if (lString.Equals(language))
            {
                return true;
            }
        }
        return false;
    }
}
