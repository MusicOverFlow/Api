namespace Api.Handlers.Queries.PostQueries;

public class CreatePostCommand : HandlerBase, Command<Task<Post>, CreatePostDto>
{
    public CreatePostCommand(ModelsContext context) : base(context)
    {
        
    }

    public async Task<Post> Handle(CreatePostDto message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.CreatorMailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        if (string.IsNullOrWhiteSpace(message.Content))
        {
            throw new HandlerException(ErrorType.PostTitleOrContentEmpty);
        }

        if (!string.IsNullOrWhiteSpace(message.ScriptLanguage))
        {
            message.ScriptLanguage = message.ScriptLanguage.ToLower();
            if (!this.IsScriptLanguageSupported(message.ScriptLanguage))
            {
                throw new HandlerException(ErrorType.WrongFormatFile);
            }
        }
        else
        {
            message.ScriptLanguage = null;
        }

        Group group = null;
        if (message.GroupId != null)
        {
            group = await this.context.Groups
                .FirstOrDefaultAsync(g => g.Id.Equals(message.GroupId));

            if (group == null)
            {
                throw new HandlerException(ErrorType.GroupNotFound);
            }
        }

        Post post = new Post()
        {
            Title = message.Title ?? "No title",
            Content = message.Content,
            CreatedAt = DateTime.Now,

            ScriptLanguage = message.ScriptLanguage,

            Owner = account,
            Group = group,

            MusicUrl = null,
        };

        this.context.Posts.Add(post);
        post.ScriptUrl = message.Script != null ? Blob.GetPostScriptUrl(message.Script, post.Id).Result : null;
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
