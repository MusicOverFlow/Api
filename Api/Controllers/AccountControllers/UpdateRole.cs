namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("role"), AuthorizeEnum(Role.Admin)]
    public async Task<ActionResult> UpdateRole(string mailAddress, string role)
    {
        /*
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(ExceptionHandler.Get(ErrorType.AccountNotFound));
        }

        string newRole = RoleParser.Handle(role);

        if (string.IsNullOrWhiteSpace(newRole))
        {
            return BadRequest(ExceptionHandler.Get(ErrorType.InvalidRole));
        }

        account.Role = newRole;

        await this.context.SaveChangesAsync();

        return Ok();
        */
        return Ok();
    }
}
