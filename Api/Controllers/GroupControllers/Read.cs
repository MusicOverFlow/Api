using Api.Models.ExpositionModels.Resources;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<GroupResource>>> Read(Guid? id = null)
    {
        /*
        IQueryable<Group> query = this.context.Groups;

        if (id != null)
        {
            query = query.Where(g => g.Id.Equals(id));
        }
        
        List<GroupResource_WithMembers_AndPosts> groups = await query
            .Select(g => this.mapper.Group_ToResource_WithMembers_AndPosts(g))
            .ToListAsync();

        if (id != null && groups.Count == 0)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.GroupNotFound));
        }

        return Ok(groups);
        */
        return Ok();
    }
}
