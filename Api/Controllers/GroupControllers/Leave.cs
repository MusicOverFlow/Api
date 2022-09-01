using Api.Handlers.Commands.GroupCommands;
using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost("leave"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> Leave(Guid groupId)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            await this.handlers.Get<LeaveGroupCommand>().Handle(new LeaveGroupDto()
            {
                MailAddress = mailAddress,
                GroupId = groupId
            });

            return Ok();
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
