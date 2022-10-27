namespace Api.Handlers.Queries.PostQueries;

public class ReadPostByAccountQuery : HandlerBase, Query<Task<List<Post>>, string>
{
    private readonly IContainer container;

    public ReadPostByAccountQuery(ModelsContext context, IContainer container) : base(context)
    {
        this.container = container;
    }
    
    public async Task<List<Post>> Handle(string message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        List<Post> posts = new List<Post>();

        account.OwnedPosts.ToList().ForEach(p =>
        {
            if (!this.Contains(posts, p.Id)) posts.Add(p);
        });
        account.OwnedCommentaries.ToList().ForEach(c =>
        {
            if (!this.Contains(posts, c.Post.Id)) posts.Add(c.Post);
        });

        posts.ForEach(async p =>
        {
            if (p.ScriptLanguage != null) p.Script = await this.container.GetScriptContent(p.Id);
        });

        return posts;
    }

    private bool Contains(List<Post> posts, Guid id)
    {
        bool contains = false;
        posts.ForEach(p =>
        {
            if (p.Id.Equals(id)) contains = true;
        });
        return contains;
    }
}
