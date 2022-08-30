using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost("leave"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> Leave(Guid groupId)
    {
        /*
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        Group group = await this.context.Groups
            .FirstOrDefaultAsync(g => g.Id.Equals(groupId));

        if (group == null)
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.GroupNotFound));
        }

        if (group.Owner.Equals(account))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.LeaveWhileOwner));
        }

        group.Members.Remove(account);
        account.Groups.Remove(group);

        await this.context.SaveChangesAsync();

        return Ok();
        */
        return Ok();
    }
}
