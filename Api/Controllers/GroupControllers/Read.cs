namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet]
    public async Task<ActionResult<GroupResource>> Read()
    {
        List<Group> groups = await this.context.Groups
            .ToListAsync();

        List<GroupResource> groupResources = new List<GroupResource>();

        groups.ForEach(g => groupResources.Add(this.mapper.Group_ToResource(g)));

        return Ok(groupResources);
    }
}
