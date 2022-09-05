using Api.Handlers.Commands.GroupCommands;
using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost("kick"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> Kick(Guid groupId, string memberMailAddress)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            Group group = await this.handlers.Get<KickMemberFromGroupCommand>().Handle(new KickMemberDto()
            {
                CallerMailAddress = mailAddress,
                MemberMailAddress = memberMailAddress,
                GroupId = groupId,
            });
            
            return Ok(Mapper.GroupToResource(group));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }        
}
