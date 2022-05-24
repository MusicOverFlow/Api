namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet("posts")]
    public async Task<ActionResult<List<PostResource>>> ReadGroupPosts(Guid? groupId)
    {
        Group group = await this.context.Groups
            .FirstOrDefaultAsync(p => p.Id.Equals(groupId));

        if (group == null)
        {
            return NotFound(new { message = "Group not found" });
        }

        return Ok(this.mapper.Group_ToResource_WithPosts(group));
    }
}
