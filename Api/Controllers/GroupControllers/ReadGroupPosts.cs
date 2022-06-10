namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet("posts")]
    public async Task<ActionResult<List<GroupResource_WithMembers_AndPosts>>> ReadGroupPosts(Guid? groupId)
    {
        Group group = await this.context.Groups
            .Include(g => g.Owner)
            .Include(g => g.Members)
            .Include(g => g.Posts)
            .FirstOrDefaultAsync(p => p.Id.Equals(groupId));

        if (group == null)
        {
            return NotFound(this.exceptionHandler.GetException(BadRequestType.GroupNotFound));
        }

        return Ok(this.mapper.Group_ToResource_WithPosts(group));
    }
}
