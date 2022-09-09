namespace Api.Handlers.Commands.PostCommands;

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

        if (!string.IsNullOrWhiteSpace(message.ScriptLanguage) && !string.IsNullOrWhiteSpace(message.Script))
        {
            if (!this.IsScriptLanguageSupported(message.ScriptLanguage.ToLower()))
            {
                throw new HandlerException(ErrorType.WrongFormatFile);
            }
        }
        else
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
            CreatedAt = DateTime.Now,

            ScriptLanguage = message.ScriptLanguage,

            Owner = account,
            Group = group,

            MusicUrl = null,
        };

        this.context.Posts.Add(post);
        post.ScriptUrl = message.Script != null ? await Blob.GetPostScriptUrl(message.Script, post.Id) : null;
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
