using Api.Models.ExpositionModels.Resources;
using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost("kick"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<GroupResource>> Kick(Guid groupId, string memberMailAddress)
    {
        /*
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account callerAccount = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (callerAccount == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        Group group = await this.context.Groups
            .FirstOrDefaultAsync(p => p.Id.Equals(groupId));

        if (group == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.GroupNotFound));
        }

        if (!group.Owner.Id.Equals(callerAccount.Id))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.NotOwnerOfGroup));
        }

        Account memberAccount = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(memberMailAddress));

        if (memberAccount == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        if (!group.Members.Any(m => m.MailAddress.Equals(memberMailAddress)))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.AccountNotInGroup));
        }

        group.Members.Remove(memberAccount);

        await this.context.SaveChangesAsync();

        return Ok(this.mapper.Group_ToResource_WithMembers(group));
        */
        return Ok();
    }        
}
