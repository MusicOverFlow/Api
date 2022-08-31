namespace Api.Handlers.Commands.CommentaryCommands;

public class CreateCommentaryCommand : HandlerBase, Command<Task<Post>, CreateCommentaryDto>
{
    public CreateCommentaryCommand(ModelsContext context) : base(context)
    {

    }

    public async Task<Post> Handle(CreateCommentaryDto createCommentary)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(createCommentary.CreatorMailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        if (string.IsNullOrWhiteSpace(createCommentary.Content))
        {
            throw new HandlerException(ErrorType.PostTitleOrContentEmpty);
        }

        Post post = await this.context.Posts
            .FirstOrDefaultAsync(p => p.Id.Equals(createCommentary.PostId));

        if (post == null)
        {
            throw new HandlerException(ErrorType.PostOrCommentaryNotFound);
        }

        if (!string.IsNullOrWhiteSpace(createCommentary.ScriptLanguage))
        {
            createCommentary.ScriptLanguage = createCommentary.ScriptLanguage.ToLower();
            if (!this.IsScriptLanguageSupported(createCommentary.ScriptLanguage))
            {
                throw new HandlerException(ErrorType.WrongFormatFile);
            }
        }
        else
        {
            createCommentary.ScriptLanguage = null;
        }

        Commentary commentary = new Commentary
        {
            Content = createCommentary.Content,
            CreatedAt = DateTime.Now,

            ScriptLanguage = createCommentary.ScriptLanguage,

            Owner = account,
            Post = post,
        };

        this.context.Commentaries.Add(commentary);
        post.Commentaries.Add(commentary);
        commentary.ScriptUrl = createCommentary.Script != null ? Blob.GetPostScriptUrl(createCommentary.Script, commentary.Id).Result : null;

        await this.context.SaveChangesAsync();

        return post;
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

