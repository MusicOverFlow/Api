namespace Api.Controllers.PostControllers;

public partial class PostController
{
    [HttpGet]
    public async Task<ActionResult<List<PostResource_WithCommentaries_AndLikes>>> Read(Guid? id = null)
    {
        IQueryable<Post> query = this.context.Posts
            .Include(p => p.Owner)
            .Include(p => p.Group)
            .Include(p => p.Commentaries)
            .Include(p => p.Likes);
        
        if (id != null)
        {
            query = query.Where(p => p.Id.Equals(id));
        }

        List<PostResource_WithCommentaries_AndLikes> posts = new List<PostResource_WithCommentaries_AndLikes>();

        await query.ForEachAsync(p => posts.Add(this.mapper.Post_ToResource_WithCommentaries_AndLikes(p)));

        if (id != null && posts.Count == 0)
        {
            return NotFound(this.exceptionHandler.PostNotFound);
        }

        return Ok(posts);
    }
}
