using Api.Handlers.Commands.PostCommands;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpPut("addMusic"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> AddMusic(Guid? id)
    {
        try
        {
            Post post = await this.handlers.Get<AddMusicPostCommand>().Handle(new AddMusicDto()
            {
                PostId = id.Value,
                File = Request.Form.Files[0],
            });

            return Ok(Mapper.PostToResource(post));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
