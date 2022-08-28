namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPost]
    public async Task<ActionResult<AccountResource>> Create(
        [FromForm] string mailAddress,
        [FromForm] string password,
        [FromForm] string firstname,
        [FromForm] string lastname,
        [FromForm] string pseudonym,
        [FromForm] byte[] profilPic)
    {
        if (!this.dataValidator.IsMailAddressValid(mailAddress))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.InvalidMail));
        }

        if (!this.dataValidator.IsPasswordValid(password))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.InvalidPassword));
        }

        bool isMailAlreadyInUse = await this.context.Accounts
            .AnyAsync(a => a.MailAddress.Equals(mailAddress));

        if (isMailAlreadyInUse)
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.MailAlreadyInUse));
        }
        
        this.EncryptPassword(password, out byte[] hash, out byte[] salt);

        byte[] fileBytes = null;
        if (Request != null)
        {
            IFormFile file = Request.Form.Files.GetFile(nameof(profilPic));
            if (file != null && file.Length > 0)
            {
                if (!this.dataValidator.IsImageFormatSupported(file.FileName))
                {
                    return BadRequest(this.exceptionHandler.GetError(ErrorType.WrongFormatFile));
                }
                
                using (MemoryStream ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }
        }

        Account account = new Account()
        {
            MailAddress = mailAddress.Trim(),
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = Role.User.ToString(),
            Firstname = firstname ?? "Unknown",
            Lastname = lastname ?? "Unknown",
            Pseudonym = pseudonym ?? "Anonymous",
            PicUrl = this.blob.GetProfilPicUrl(fileBytes, mailAddress.Trim()).Result,
            CreatedAt = DateTime.Now,
        };

        this.context.Accounts.Add(account);
        
        await this.context.SaveChangesAsync();
        
        return Created(nameof(Create), this.mapper.Account_ToResource(account));
    }
}
