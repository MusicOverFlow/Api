using Api.Handlers.Kernel;
using Api.Models.ExpositionModels.Requests;
using System.Security.Cryptography;

namespace Api.Handlers.Commands;

public class CreateAccountCommand : HandlerBase, Command<Task<Account>, CreateAccountRequest>
{
    public CreateAccountCommand(ModelsContext context) : base(context)
    {

    }

    public async Task<Account> Handle(CreateAccountRequest accountRequest)
    {
        if (!DataValidator.IsMailAddressValid(accountRequest.MailAddress))
        {
            throw new HandlerException(ErrorType.InvalidMail);
        }

        if (!DataValidator.IsPasswordValid(accountRequest.Password))
        {
            throw new HandlerException(ErrorType.InvalidPassword);
        }

        if (await this.context.Accounts
            .AnyAsync(a => a.MailAddress.Equals(accountRequest.MailAddress)))
        {
            throw new HandlerException(ErrorType.MailAlreadyInUse);
        }

        this.EncryptPassword(accountRequest.Password, out byte[] hash, out byte[] salt);

        byte[] fileBytes = null;
        if (accountRequest.ProfilPic != null && accountRequest.ProfilPic.Length > 0)
        {
            if (!DataValidator.IsImageFormatSupported(accountRequest.ProfilPic.FileName))
            {
                throw new HandlerException(ErrorType.WrongFormatFile);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                accountRequest.ProfilPic.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
        }

        Account account = new Account()
        {
            MailAddress = accountRequest.MailAddress,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = Role.User.ToString(),
            Firstname = accountRequest.Firstname ?? "Unknown",
            Lastname = accountRequest.Lastname ?? "Unknown",
            Pseudonym = accountRequest.Pseudonym ?? "Anonymous",
            PicUrl = Blob.GetProfilPicUrl(fileBytes, accountRequest.MailAddress).Result,
            CreatedAt = DateTime.Now,
        };

        this.context.Accounts.Add(account);
        await this.context.SaveChangesAsync();

        return account;
    }

    private void EncryptPassword(string password, out byte[] hash, out byte[] salt)
    {
        using HMACSHA512 hmac = new HMACSHA512();
        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        salt = hmac.Key;
    }
}
