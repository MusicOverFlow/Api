using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("mail"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> UpdateMailAddress(string mailAddress)
    {
        /*
        string actualMailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(actualMailAddress));

        if (account == null)
        {
            return NotFound(ExceptionHandler.Get(ErrorType.AccountNotFound));
        }

        if (!DataValidator.IsMailAddressValid(mailAddress))
        {
            return BadRequest(ExceptionHandler.Get(ErrorType.InvalidMail));
        }

        bool isMailAlreadyInUse = await this.context.Accounts
            .AnyAsync(a => a.MailAddress.Equals(mailAddress));

        if (isMailAlreadyInUse)
        {
            return BadRequest(ExceptionHandler.Get(ErrorType.MailAlreadyInUse));
        }

        account.MailAddress = mailAddress;
        await this.context.SaveChangesAsync();

        return Ok();
        */
        return Ok();
    }
}
