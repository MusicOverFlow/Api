namespace Api.Controllers.AccountControllers;

public partial class AccountControllerBase
{
    [HttpDelete, AuthorizeEnum(Role.Admin)]
    public async Task<ActionResult> Delete(string mailAddress)
    {
        Account account = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        this.context.Accounts.Remove(account);

        await this.context.SaveChangesAsync();

        return Ok();
    }
}
