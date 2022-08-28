using System.Security.Claims;

namespace Api.Controllers.CommentaryControllers;

public partial class CommentaryController
{
    [HttpPost, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<PostResource>> Create(CreateCommentary request, Guid? postId)
    {
        string mailAddress = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

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

        Post post = await this.context.Posts
            .FirstOrDefaultAsync(p => p.Id.Equals(postId));

        if (post == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.PostOrCommentaryNotFound));
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

        Commentary commentary = new Commentary
        {
            Content = request.Content,
            CreatedAt = DateTime.Now,

            ScriptLanguage = request.ScriptLanguage,

            Owner = account,
            Post = post,
        };

        this.context.Commentaries.Add(commentary);
        post.Commentaries.Add(commentary);

        commentary.ScriptUrl = request.Script != null ? this.blob.GetPostScriptUrl(request.Script, commentary.Id).Result : null;

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
