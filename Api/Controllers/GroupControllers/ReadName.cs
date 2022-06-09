namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet("name")]
    public async Task<ActionResult<List<GroupResource_WithMembers>>> ReadName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest();
        }

        List<GroupResource_WithMembers> groups = new List<GroupResource_WithMembers>();

        await this.context.Groups
            .Include(g => g.Owner)
            .Include(g => g.Members)
            .ForEachAsync(g =>
            {
                if (groups.Count >= this.MAX_GROUPS_IN_SEARCHES)
                {
                    return;
                }

                if (this.stringComparer.Compare(name, g.Name) >= 0.5)
                {
                    groups.Add(this.mapper.Group_ToResource_WithMembers(g));
                }
            });

        return Ok(groups);
    }
}
