namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet]
    public async Task<ActionResult<GroupResource>> Read()
    {
        List<Group> groups = await this.context.Groups
            .Include(g => g.Name)
            .Include(g => g.Description)
            .Include(g => g.PicUrl)
            .ToListAsync();

        List<GroupResource> groupResources = new List<GroupResource>();

        groups.ForEach(g => groupResources.Add(this.mapper.Group_ToResource(g)));

        return Ok(groupResources);
    }
}
