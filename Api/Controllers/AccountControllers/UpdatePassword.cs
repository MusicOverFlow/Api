using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("password"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> UpdatePassword(string password)
    {
        /*
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(ExceptionHandler.Get(ErrorType.AccountNotFound));
        }

        if (!DataValidator.IsPasswordValid(password))
        {
            return BadRequest(ExceptionHandler.Get(ErrorType.InvalidPassword));
        }

        this.EncryptPassword(password, out byte[] hash, out byte[] salt);

        account.PasswordHash = hash;
        account.PasswordSalt = salt;
        await this.context.SaveChangesAsync();

        return Ok();
        */
        return Ok();
    }
}
