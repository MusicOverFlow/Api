using Api.Handlers.Commands.CommentaryCommands;
using Api.Models.ExpositionModels.Resources;
using System.Security.Claims;

namespace Api.Controllers.CommentaryControllers;

public partial class CommentaryController
{
    [HttpPost, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> Create(CreateCommentary request, Guid? postId)
    {
        string mailAddress = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            Post post = await this.handlers.Get<CreateCommentaryCommand>().Handle(new CreateCommentaryDto()
            {
                CreatorMailAddress = mailAddress,
                Content = request.Content,
                PostId = postId.Value,
                ScriptLanguage = request.ScriptLanguage,
                Script = request.Script,
            });

            return Created(nameof(Create), Mapper.PostToResource(post));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
