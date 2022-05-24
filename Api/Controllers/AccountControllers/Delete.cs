namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpDelete, AuthorizeEnum(Role.Admin)]
    public async Task<ActionResult> Delete(string mailAddress)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        this.context.Accounts.Remove(account);

        await this.context.SaveChangesAsync();

        return Ok();
    }
}
