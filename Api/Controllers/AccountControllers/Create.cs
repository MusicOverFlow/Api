namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPost]
    public async Task<ActionResult<AccountResource>> Create(CreateAccountRequest request)
    {
        if (!this.dataValidator.IsMailAddressValid(request.MailAddress))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.InvalidMail));
        }

        if (!this.dataValidator.IsPasswordValid(request.Password))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.InvalidPassword));
        }

        bool isMailAlreadyInUse = await this.context.Accounts
            .AnyAsync(a => a.MailAddress.Equals(request.MailAddress));

        if (isMailAlreadyInUse)
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.MailAlreadyInUse));
        }

        this.EncryptPassword(request.Password, out byte[] hash, out byte[] salt);

        IFormFile file = Request.Form.Files.Any() ? Request.Form.Files[0] : new FormFileCollection()[0];
        string picUrl = string.Empty;
        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            byte[] fileBytes = ms.ToArray();

            picUrl = this.GetProfilPicUrl(fileBytes, request.MailAddress.Trim()).Result;
        }

        Account account = new Account()
        {
            MailAddress = request.MailAddress.Trim(),
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = Role.Admin.ToString(),
            Firstname = request.Firstname ?? "Unknown",
            Lastname = request.Lastname ?? "Unknown",
            Pseudonym = request.Pseudonym ?? "Anonymous",
            PicUrl = picUrl,
            CreatedAt = DateTime.Now,
        };

        this.context.Accounts.Add(account);
        
        await this.context.SaveChangesAsync();
        
        return Created(nameof(Create), this.mapper.Account_ToResource(account));
    }
}
