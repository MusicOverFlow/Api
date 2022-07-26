using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("mail"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> UpdateMailAddress(string mailAddress)
    {
        string actualMailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(actualMailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        if (!this.dataValidator.IsMailAddressValid(mailAddress))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.InvalidMail));
        }

        bool isMailAlreadyInUse = await this.context.Accounts
            .AnyAsync(a => a.MailAddress.Equals(mailAddress));

        if (isMailAlreadyInUse)
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.MailAlreadyInUse));
        }

        account.MailAddress = mailAddress;
        await this.context.SaveChangesAsync();

        return Ok();
    }
}
