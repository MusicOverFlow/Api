using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpGet("posts"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<GroupResource>>> ReadGroupPosts(Guid? groupId)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        Group group = await this.context.Groups
            .FirstOrDefaultAsync(p => p.Id.Equals(groupId));

        if (group == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.GroupNotFound));
        }

        if (!account.Groups.Contains(group))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.NotMemberOfGroup));
        }

        return Ok(this.mapper.Group_ToResource_WithPosts(group));
    }
}
