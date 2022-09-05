using Api.Handlers.Commands.PostCommands;
using System.Security.Claims;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpPost, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> Create(CreatePostRequest request, Guid? groupId)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            Post post = await this.handlers.Get<CreatePostCommand>().Handle(new CreatePostDto()
            {
                CreatorMailAddress = mailAddress,
                Title = request.Title,
                Content = request.Content,
                GroupId = groupId,
                ScriptLanguage = request.ScriptLanguage,
                Script = request.Script
            });
            
            return Created(nameof(Create), Mapper.PostToResource(post));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
