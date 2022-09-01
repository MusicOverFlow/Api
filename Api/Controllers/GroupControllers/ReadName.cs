using Api.Handlers.Queries.GroupQueries;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet("name"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> ReadName(string name)
    {
        try
        {
            List<Group> groups = await this.handlers.Get<ReadGroupByNameQuery>().Handle(name);

            return Ok(groups.Select(g => Mapper.Group_ToResource(g)));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
