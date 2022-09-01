using Api.Handlers.Queries.GroupQueries;
using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet("posts"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> ReadGroupPosts(Guid? groupId)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;
        
        try
        {
            List<Post> posts = await this.handlers.Get<ReadGroupPostsQuery>().Handle(new ReadGroupPostsDto()
            {
                MailAddress = mailAddress,
                GroupId = groupId,
            });

            return Ok(posts.Select(p => Mapper.Post_ToResource(p)));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
