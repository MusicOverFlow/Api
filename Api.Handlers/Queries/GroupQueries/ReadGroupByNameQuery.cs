namespace Api.Handlers.Queries.GroupQueries;

public class ReadGroupByNameQuery : HandlerBase, Query<Task<List<Group>>, string>
{
    private const int MAX_GROUPS_IN_SEARCHES = 10;

    public ReadGroupByNameQuery(ModelsContext context) : base(context)
    {
        
    }

    public async Task<List<Group>> Handle(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new HandlerException(ErrorType.Undefined);
        }

        List<Group> groups = new List<Group>();

        await this.context.Groups
            .ForEachAsync(g =>
            {
                if (groups.Count >= MAX_GROUPS_IN_SEARCHES)
                {
                    return;
                }

                if (LevenshteinDistance.Compare(message, g.Name) >= 0.5)
                {
                    groups.Add(g);
                }
            });

        return groups;
    }
}
