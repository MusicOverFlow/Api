namespace Api.Handlers.Commands.PostCommands;

public class CreatePostCommand : HandlerBase, Command<Task<Post>, CreatePostDto>
{
    private readonly IContainer container;

    public CreatePostCommand(ModelsContext context, IContainer container) : base(context)
    {
        this.container = container;
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

        if (!string.IsNullOrWhiteSpace(message.ScriptLanguage) && !this.IsScriptLanguageSupported(message.ScriptLanguage.ToLower()))
        {
            throw new HandlerException(ErrorType.WrongFormatFile);
        }

        if (string.IsNullOrWhiteSpace(message.ScriptLanguage) || string.IsNullOrWhiteSpace(message.Script))
        {
            message.ScriptLanguage = null;
            message.Script = null;
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
            Title = !string.IsNullOrWhiteSpace(message.Title) ? message.Title : "No title",
            Content = message.Content,
            ScriptLanguage = message.ScriptLanguage != null ? message.ScriptLanguage.ToLower() : null,
            Owner = account,
            Group = group,
            MusicUrl = null,
        };

        this.context.Posts.Add(post);
        await this.context.SaveChangesAsync();

        if (post.ScriptLanguage != null)
        {
            await this.container.GetPostScriptUrl(message.Script, post.Id);
            post.Script = await this.container.GetScriptContent(post.Id);
        }

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
