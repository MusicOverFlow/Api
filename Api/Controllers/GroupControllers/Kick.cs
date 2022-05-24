using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost("kick")]
    public async Task<ActionResult<GroupResource>> Kick(Guid groupId, string memberMailAddress)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account callerAccount = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (callerAccount == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        Group group = await this.context.Groups
            .FirstOrDefaultAsync(p => p.Id.Equals(groupId));

        if (group == null)
        {
            return NotFound(new { message = "Group not found" });
        }

        if (!group.Owner.Id.Equals(callerAccount.Id))
        {
            return BadRequest(new { message = "You are not the owner of the group" });
        }

        Account memberAccount = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(memberMailAddress));

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

        return Ok(mapper.Group_ToResource_WithMembers(group));
    }        
}
