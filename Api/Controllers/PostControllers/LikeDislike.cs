using Api.Handlers.Commands.PostCommands;
using System.Security.Claims;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpPut("like"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> LikeDislike(Guid? id = null)
    {
        string callerAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            await this.handlers.Get<LikeDislikePostCommand>().Handle(new LikeDislikeDto()
            {
                CallerMail = callerAddress,
                PostId = id,
            });

            return Ok();
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
