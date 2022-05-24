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
            .Include(a => a.Groups)
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
        account.Groups.Add(group);

        await this.context.SaveChangesAsync();

        return Created(nameof(Create), this.mapper.Group_ToResource(group));
    }

    [HttpPost("join")]
    public async Task<ActionResult<GroupResource>> Join(Guid groupId)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        Group group = this.context.Groups
            .Where(p => p.Id.Equals(groupId))
            .FirstOrDefault();

        if (group == null)
        {
            return NotFound(new { message = "Group not found" });
        }

        if (group.Members.Any(m => m.Id.Equals(account.Id)))
        {
            return BadRequest(new { message = "Account already in group" });
        }

        group.Members.Add(account);

        await this.context.SaveChangesAsync();

        return Ok(this.mapper.Group_ToResource_WithMembers(group));
    }

    [HttpPost("kick")]
    public async Task<ActionResult<GroupResource>> Kick(Guid groupId, string memberMailAddress)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account callerAccount = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .FirstOrDefaultAsync();

        if (callerAccount == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        Group group = this.context.Groups
            .Where(p => p.Id.Equals(groupId))
            .FirstOrDefault();

        if (group == null)
        {
            return NotFound(new { message = "Group not found" });
        }

        if (!group.Owner.Id.Equals(callerAccount.Id))
        {
            return BadRequest(new { message = "You are not the owner of the group" });
        }

        Account memberAccount = this.context.Accounts
            .Where(a => a.MailAddress.Equals(memberMailAddress))
            .FirstOrDefault();

        if (memberAccount == null)
        {
            return NotFound(new { message = "Member account not found" });
        }

        if (!group.Members.Any(m => m.MailAddress.Equals(memberMailAddress)))
        {
            return BadRequest(new { message = "This account is not part of the group" });
        }

        group.Members.Remove(memberAccount);

        await this.context.SaveChangesAsync();

        return Ok(this.mapper.Group_ToResource_WithMembers(group));
    }

    [HttpGet("name")]
    public async Task<ActionResult<List<GroupResource>>> ReadName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new { message = "Invalid name" });
        }

        List<GroupResource> groupResources = new List<GroupResource>();

        await this.context.Groups
            .Include(g => g.Owner)
            .ForEachAsync(g =>
            {
                if (groupResources.Count >= this.MAX_ACCOUNT_IN_SEARCHES)
                {
                    return;
                }

                if (this.stringComparer.Compare(name, g.Name) >= 0.5)
                {
                    groupResources.Add(this.mapper.Group_ToResource(g));
                }
            });

        return Ok(groupResources);
    }

    [HttpGet("posts")]
    public async Task<ActionResult<List<PostResource>>> ReadGroupPosts(Guid? groupId)
    {
        Group group = await this.context.Groups
            .Where(p => p.Id.Equals(groupId))
            .FirstOrDefaultAsync();

        if (group == null)
        {
            return NotFound(new { message = "Group not found" });
        }

        return Ok(this.mapper.Group_ToResource_WithPosts(group));
    }
}
