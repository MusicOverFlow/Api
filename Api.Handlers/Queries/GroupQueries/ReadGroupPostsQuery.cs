namespace Api.Handlers.Queries.GroupQueries;

public class ReadGroupPostsQuery : HandlerBase, Query<Task<List<Post>>, ReadGroupPostsDto>
{
    public ReadGroupPostsQuery(ModelsContext context) : base(context)
    {
        
    }

    public async Task<List<Post>> Handle(ReadGroupPostsDto message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        Group group = await this.context.Groups
            .FirstOrDefaultAsync(p => p.Id.Equals(message.GroupId));

        if (group == null)
        {
            throw new HandlerException(ErrorType.GroupNotFound);
        }

        if (!account.Groups.Contains(group))
        {
            throw new HandlerException(ErrorType.NotMemberOfGroup);
        }
        
        return group.Posts
            .Select(p => p)
            .ToList();
    }
}
