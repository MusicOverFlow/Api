using System.Security.Claims;

namespace Api.Controllers.PostControllers;

[Route("api/posts")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly Mapper mapper;

    public PostsController(
        ModelsContext context,
        Mapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<PostResource>> Create(CreatePost request, Guid? groupId)
    {
        string mailAddress = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { errorMessage = "Account not found" });
        }

        Group group = null;
        if (groupId != null)
        {
            group = context.Groups
                .Where(g => g.Id.Equals(groupId))
                .FirstOrDefault();

            if (group == null)
            {
                return NotFound(new { errorMessage = "Group not found" });
            }
        }

        Post post = new Post()
        {
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.Now,

            Owner = account,
            Commentaries = new List<Commentary>(),
            Group = group,
        };

        context.Posts.Add(post);

        await context.SaveChangesAsync();

        return Created(nameof(Create), mapper.Post_ToResource(post));
    }

    [HttpGet]
    public async Task<ActionResult<List<PostResource>>> Read(Guid? id = null)
    {
        IQueryable<Post> query = context.Posts
            .Include(p => p.Owner)
            .Include(p => p.Group)
            .Include(p => p.Likes);

        if (id != null)
        {
            query = query.Where(a => a.Id.Equals(id));
        }

        List<PostResource> posts = new List<PostResource>();

        await query.ForEachAsync(p => posts.Add(mapper.Post_ToResource(p)));

        if (id != null && posts.Count == 0)
        {
            return NotFound(new { message = "Post not found" });
        }

        return Ok(posts);
    }

    [HttpGet("byAccount")]
    public async Task<ActionResult<List<PostResource>>> ReadAccountPosts()
    {
        string mailAddress = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .Include(a => a.OwnedPosts)
                .ThenInclude(p => p.Group)
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { errorMessage = "Account not found" });
        }

        return Ok(mapper.Account_ToResource_WithPosts(account));
    }
}
