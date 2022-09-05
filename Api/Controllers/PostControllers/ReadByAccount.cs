using Api.Handlers.Queries.PostQueries;
using System.Security.Claims;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet("byAccount"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> ReadByAccount()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        try
        {
            List<Post> posts = await this.handlers.Get<ReadPostByAccountQuery>().Handle(mailAddress);

            return Ok(posts
                .Select(p => Mapper.PostToResource(p))
                .OrderByDescending(p => p.CreatedAt)
                .ToList());
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
