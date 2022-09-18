using Api.Handlers.Commands.GroupCommands;
using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> Create(
        [FromForm] string name,
        [FromForm] string description,
        [FromForm] byte[] groupPic)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            Group group = await this.handlers.Get<CreateGroupCommand>().Handle(new CreateGroupDto()
            {
                CreatorMailAddress = mailAddress,
                Name = name,
                Description = description,
                GroupPic = Request.Form.Files.GetFile(nameof(groupPic)),
            });

            return Ok();
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
