using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost("join")]
    public async Task<ActionResult<GroupResource>> Join(Guid groupId)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.AccountNotFound);
        }

        Group group = await this.context.Groups
            .Include(g => g.Members)
            .FirstOrDefaultAsync(p => p.Id.Equals(groupId));

        if (group == null)
        {
            return NotFound(this.exceptionHandler.GroupNotFound);
        }

        if (group.Members.Any(m => m.Id.Equals(account.Id)))
        {
            return BadRequest(this.exceptionHandler.AccountAlreadyInGroup);
        }

        group.Members.Add(account);

        await this.context.SaveChangesAsync();

        return Ok(this.mapper.Group_ToResource(group));
    }
}
