namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet]
    public async Task<ActionResult<List<PostResource>>> Read(Guid? id = null)
    {
        IQueryable<Post> query = this.context.Posts;
        
        if (id != null)
        {
            query = query.Where(p => p.Id.Equals(id));
        }

        List<PostResource> posts = new List<PostResource>();

        await query.ForEachAsync(p => posts.Add(this.mapper.Post_ToResource(p)));

        if (id != null && posts.Count == 0)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.PostNotFound));
        }

        return Ok(posts);
    }
}
