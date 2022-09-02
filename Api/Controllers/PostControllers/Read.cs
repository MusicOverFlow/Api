using Api.Handlers.Queries.PostQueries;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> Read(Guid? id = null)
    {
        try
        {
            List<Post> posts = await this.handlers.Get<ReadPostByIdQuery>().Handle(id);

            return Ok(posts
                .Select(p => Mapper.Post_ToResource(p))
                .ToList());
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
