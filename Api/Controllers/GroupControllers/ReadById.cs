namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet("id")]
    public async Task<ActionResult<List<GroupResource_WithMembers>>> ReadById(Guid id)
    {
        Group group = await this.context.Groups
            .Include(g => g.Posts.OrderBy(g => g.CreatedAt))
            .FirstOrDefaultAsync(g => g.Id.Equals(id));

        if (group == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.GroupNotFound));
        }

        return Ok(this.mapper.Group_ToResource_WithPosts(group));
    }
}
