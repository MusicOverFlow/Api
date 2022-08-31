using Api.Handlers.Dtos;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("profil"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> UpdateProfil(UpdateProfilRequest request)
    {
        /*
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(ExceptionHandler.Get(ErrorType.AccountNotFound));
        }

        account.Firstname = !string.IsNullOrWhiteSpace(request.Firstname) ? request.Firstname : account.Firstname;
        account.Lastname = !string.IsNullOrWhiteSpace(request.Lastname) ? request.Lastname : account.Lastname;
        account.Pseudonym = !string.IsNullOrWhiteSpace(request.Pseudonym) ? request.Pseudonym : account.Pseudonym;
        await this.context.SaveChangesAsync();

        return Ok();
        */
        return Ok();
    }
}
