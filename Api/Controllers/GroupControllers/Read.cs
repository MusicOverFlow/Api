namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet]
    public async Task<ActionResult<GroupResource>> Read()
    {
        List<Group> groups = await this.context.Groups
            .Include(g => g.Owner)
            .Include(g => g.Members)
            .ToListAsync();

        return Ok(groups.Select(g => this.mapper.Group_ToResource(g)));
    }
}
