using Api.Handlers.Queries.GroupQueries;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> Read(Guid? id = null)
    {
        try
        {
            List<Group> groups = await this.handlers.Get<ReadGroupByIdQuery>().Handle(id);

            return Ok(groups
                .Select(a => Mapper.GroupToResource(a))
                .ToList());
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
