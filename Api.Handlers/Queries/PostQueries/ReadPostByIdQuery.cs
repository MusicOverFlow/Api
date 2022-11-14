using Api.Models.Entities;

namespace Api.Handlers.Queries.PostQueries;

public class ReadPostByIdQuery : HandlerBase, Query<Task<List<Post>>, Guid?>
{
    private readonly IContainer container;

    public ReadPostByIdQuery(ModelsContext context, IContainer container) : base(context)
    {
        this.container = container;
    }

    public async Task<List<Post>> Handle(Guid? message)
    {
        IQueryable<Post> query = this.context.Posts;

        if (message != null)
        {
            query = query.Where(p => p.Id.Equals(message));
        }

        List<Post> posts = await query.ToListAsync();

        if (message != null && posts.Count == 0)
        {
            throw new HandlerException(ErrorType.PostNotFound);
        }

        return posts;
    }
}
