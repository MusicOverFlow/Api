namespace Api.Handlers.Queries.GroupQueries;

public class ReadGroupQuery : HandlerBase, Query<Task<List<Group>>, Guid?>
{
    public ReadGroupQuery(ModelsContext context) : base(context)
    {
        
    }

    public async Task<List<Group>> Handle(Guid? message = null)
    {
        IQueryable<Group> query = this.context.Groups;

        if (message != null)
        {
            query = query.Where(g => g.Id.Equals(message));
        }
        
        List<Group> groups = await query
            .ToListAsync();

        if (message != null && groups.Count == 0)
        {
            throw new HandlerException(ErrorType.GroupNotFound);
        }

        return groups;
    }
}
