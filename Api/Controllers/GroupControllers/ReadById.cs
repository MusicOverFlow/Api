namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet("id"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<GroupResource>>> ReadById(Guid id)
    {
        Group group = await this.context.Groups
            .Include(g => g.Owner)
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id.Equals(id));

        if (group == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.GroupNotFound));
        }

        return Ok(this.mapper.Group_ToResource_WithMembers(group));
    }
}
