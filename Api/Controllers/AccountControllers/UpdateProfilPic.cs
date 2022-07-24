using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("profilpic"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<AccountResource>> UpdateProfilPic(UpdateProfilPic request)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        account.ProfilPicUrl = this.GetProfilPicUrl(request.ProfilPic, account.MailAddress).Result;
        await this.context.SaveChangesAsync();

        return Ok(this.mapper.Account_ToResource(account));
    }
}
