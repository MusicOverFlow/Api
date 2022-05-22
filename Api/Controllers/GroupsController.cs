using Api.Models;
using Api.Models.Entities;
using Api.Utilitaries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/groups")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly int MAX_ACCOUNT_IN_SEARCHES = 20;

    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly Utilitaries.StringComparer stringComparer;

    public GroupsController(
        ModelsContext context,
        Mapper mapper,
        Utilitaries.StringComparer stringComparer)
    {
        this.context = context;
        this.mapper = mapper;
        this.stringComparer = stringComparer;
    }

    [HttpPost]
    public async Task<ActionResult<GroupResource>> Create(CreateGroup request)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        // TODO: words ban list ?
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "Group name is required" });
        }

        Group group = new Group()
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.Now,
            
            Owner = account,
            Members = new List<Account>(),
            Posts = new List<Post>(),
        };

        this.context.Groups.Add(group);
        
        await this.context.SaveChangesAsync();

        return Created(nameof(Create), this.mapper.GroupToResource(group));
    }

    [HttpGet("name")]
    public async Task<ActionResult<GroupResource>> ReadName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new { message = "Invalid name" });
        }

        List<GroupResource> groupResources = new List<GroupResource>();

        await this.context.Groups
            .Include(g => g.Owner)
            .Include(g => g.Members)
            .Include(g => g.Posts)
            .ForEachAsync(g =>
            {
                if (groupResources.Count >= this.MAX_ACCOUNT_IN_SEARCHES)
                {
                    return;
                }

                if (this.stringComparer.Compare(name, g.Name) >= 0.5)
                {
                    groupResources.Add(this.mapper.GroupToResource(g));
                }
            });

        return Ok(groupResources);
    }
}
