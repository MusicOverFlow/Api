using Api.Handlers.Queries.PostQueries;
using System.Security.Claims;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet("homePage"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> ReadHomePage()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            List<Post> posts = await this.handlers.Get<ReadHomePagePostsQuery>().Handle(mailAddress);

            return Ok(posts.Select(p => Mapper.Post_ToResource(p)).ToList());
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
