using Api.Models.Entities;

namespace Api.Handlers.Commands.CommentaryCommands;

public class CreateCommentaryCommand : HandlerBase, Command<Task<Post>, CreateCommentaryDto>
{
    private readonly IContainer container;
    
    public CreateCommentaryCommand(ModelsContext context, IContainer container) : base(context)
    {
        this.container = container;
    }

    public async Task<Post> Handle(CreateCommentaryDto message)
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

        Post post = await this.context.Posts
            .FirstOrDefaultAsync(p => p.Id.Equals(message.PostId));

        if (post == null)
        {
            throw new HandlerException(ErrorType.PostOrCommentaryNotFound);
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

        Commentary commentary = new Commentary
        {
            Content = message.Content,
            ScriptLanguage = message.ScriptLanguage != null ? message.ScriptLanguage.ToLower() : null,
            Script = message.Script,
            Owner = account,
            Post = post,
        };

        this.context.Commentaries.Add(commentary);

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

