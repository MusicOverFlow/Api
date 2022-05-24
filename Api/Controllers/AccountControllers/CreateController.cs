using System.Security.Cryptography;

namespace Api.Controllers.AccountControllers;

public partial class AccountControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AccountResource>> Create(CreateAccountRequest request)
    {
        if (!this.dataValidator.IsMailAddressValid(request.MailAddress))
        {
            return BadRequest(new { message = "Invalid mail address" });
        }

        if (!this.dataValidator.IsPasswordValid(request.Password))
        {
            return BadRequest(new { message = "Invalid password" });
        }

        bool isMailAlreadyInUse = await this.context.Accounts
            .AnyAsync(a => a.MailAddress.Equals(request.MailAddress));

        if (isMailAlreadyInUse)
        {
            return BadRequest(new { message = "Mail already in use" });
        }

        EncryptPassword(request.Password, out byte[] hash, out byte[] salt);

        Account account = new Account()
        {
            MailAddress = request.MailAddress.Trim(),
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = Role.Admin.ToString(),
            Firstname = request.Firstname ?? "Unknown",
            Lastname = request.Lastname ?? "Unknown",
            CreatedAt = DateTime.Now,
        };

        this.context.Accounts.Add(account);
        
        await this.context.SaveChangesAsync();
        
        return Created(nameof(Create), this.mapper.Account_ToResource(account));
    }

    private void EncryptPassword(string password, out byte[] hash, out byte[] salt)
    {
        using HMACSHA512 hmac = new HMACSHA512();

        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        salt = hmac.Key;
    }
}
