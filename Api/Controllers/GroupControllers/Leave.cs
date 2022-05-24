﻿using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost("leave")]
    public async Task<ActionResult> Leave(Guid groupId)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return BadRequest(new { message = "Account not found" });
        }

        Group group = await this.context.Groups
            .Include(g => g.Owner)
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id.Equals(groupId));

        if (group == null)
        {
            return BadRequest(new { message = "Group not found" });
        }

        if (group.Owner.Equals(account))
        {
            return BadRequest(new { message = "You can't leave a group which you are the owner of" });
        }

        group.Members.Remove(account);
        account.Groups.Remove(group);

        await this.context.SaveChangesAsync();

        return Ok();
    }
}