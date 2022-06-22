namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("role"), AuthorizeEnum(Role.Admin)]
    public async Task<ActionResult<AccountResource>> UpdateRole(string mailAddress, string role)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        string newRole = RoleParser.Handle(role);

        if (string.IsNullOrWhiteSpace(newRole))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.InvalidRole));
        }

        account.Role = newRole;

        await this.context.SaveChangesAsync();

        return Ok(this.mapper.Account_ToResource(account));
    }
}
