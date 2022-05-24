namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet("name")]
    public async Task<ActionResult<List<GroupResource>>> ReadName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new { message = "Invalid name" });
        }

        List<GroupResource> groupResources = new List<GroupResource>();

        await this.context.Groups
            .Include(g => g.Owner)
            .ForEachAsync(g =>
            {
                if (groupResources.Count >= this.MAX_GROUPS_IN_SEARCHES)
                {
                    return;
                }

                if (this.stringComparer.Compare(name, g.Name) >= 0.5)
                {
                    groupResources.Add(mapper.Group_ToResource(g));
                }
            });

        return Ok(groupResources);
    }
}
