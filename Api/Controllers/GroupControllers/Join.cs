using Api.Handlers.Commands.GroupCommands;
using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost("join"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> Join(Guid groupId)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;
        
        try
        {
            Group group = await this.handlers.Get<JoinGroupCommand>().Handle(new JoinGroupDto()
            {
                MailAddress = mailAddress,
                GroupId = groupId
            });

            return Ok(Mapper.Group_ToResource(group));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
