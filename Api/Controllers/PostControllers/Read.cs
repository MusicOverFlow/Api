using Api.Models.ExpositionModels.Resources;

namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<PostResource>>> Read(Guid? id = null)
    {
        /*
        IQueryable<Post> query = this.context.Posts;
        
        if (id != null)
        {
            query = query.Where(p => p.Id.Equals(id));
        }

        List<PostResource> posts = await query
            .Select(p => this.mapper.Post_ToResource(p))
            .ToListAsync();

        if (id != null && posts.Count == 0)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.PostNotFound));
        }

        return Ok(posts);
        */
        return Ok();
    }
}
