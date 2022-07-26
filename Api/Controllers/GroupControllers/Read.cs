namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet]
    public async Task<ActionResult<GroupResource>> Read()
    {
        // TODO: REFACTO ICI
        List<Group> groups = await this.context.Groups
            .ToListAsync();

        return Ok(groups.Select(g => this.mapper.Group_ToResource(g)));
    }
}
