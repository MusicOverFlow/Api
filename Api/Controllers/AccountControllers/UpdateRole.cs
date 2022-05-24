namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("role"), AuthorizeEnum(Role.Admin)]
    public async Task<ActionResult<AccountResource>> UpdateRole(string mailAddress, string role)
    {
        if (string.IsNullOrWhiteSpace(mailAddress) || string.IsNullOrWhiteSpace(role))
        {
            return BadRequest(new { message = "Malformed request" });
        }

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        string newRole = RoleParser.Handle(role);

        if (string.IsNullOrWhiteSpace(newRole))
        {
            return BadRequest(new { message = $"Role {role} does not exist" });
        }

        account.Role = newRole;

        await this.context.SaveChangesAsync();

        return Ok(this.mapper.Account_ToResource(account));
    }
}
