namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet]
    public async Task<ActionResult<List<PostResource>>> Read(Guid? id = null)
    {
        IQueryable<Post> query = this.context.Posts
            .Include(p => p.Owner)
            .Include(p => p.Group)
            .Include(p => p.Likes);

        if (id != null)
        {
            query = query.Where(a => a.Id.Equals(id));
        }

        List<PostResource> posts = new List<PostResource>();

        await query.ForEachAsync(p => posts.Add(this.mapper.Post_ToResource(p)));

        if (id != null && posts.Count == 0)
        {
            return NotFound(new { message = "Post not found" });
        }

        return Ok(posts);
    }
}
