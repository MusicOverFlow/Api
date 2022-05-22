using Api.Models;
using Api.Models.Entities;
using Api.Utilitaries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Api.Wrappers.AuthorizeRolesAttribute;

namespace Api.Controllers;

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
    public async Task<ActionResult<PostResource>> Create(CreatePost request)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { errorMessage = "Account not found" });
        }

        Group group = null;
        if (request.GroupId != null)
        {
            group = this.context.Groups
                .Where(g => g.Id.Equals(request.GroupId))
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
            
            Account = account,
            Commentaries = new List<Commentary>(),
            Group = group,
        };

        this.context.Posts.Add(post);

        await this.context.SaveChangesAsync();

        return Created(nameof(Create), this.mapper.PostToResource(post));
    }

    [HttpGet]
    public async Task<ActionResult<List<AccountResource>>> Read(Guid? id = null)
    {
        IQueryable<Post> query = this.context.Posts
            .Include(p => p.Account)
            .Include(p => p.Commentaries)
                .ThenInclude(c => c.Account);

        if (id != null)
        {
            query = query.Where(a => a.Id.Equals(id));
        }

        List<PostResource> posts = new List<PostResource>();
        
        await query.ForEachAsync(p => posts.Add(this.mapper.PostToResourceWithCommentaries(p)));

        if (id != null && posts.Count == 0)
        {
            return NotFound(new { message = "Post not found" });
        }

        return Ok(posts);
    }

    [HttpGet("byAccount")]
    public async Task<ActionResult<List<PostResource>>> ReadAccountPosts(string mailAddress)
    {
        Account account = await this.context.Accounts
            .Include(a => a.Posts)
                .ThenInclude(p => p.Commentaries)
            .Include(p => p.Commentaries)
            .Where(a => a.MailAddress.Equals(mailAddress))
            .FirstOrDefaultAsync();
        
        if (account == null)
        {
            return NotFound(new { errorMessage = "Account not found" });
        }

        return Ok(this.mapper.AccountToResourceWithPostsAndCommentaries(account));
    }
}
