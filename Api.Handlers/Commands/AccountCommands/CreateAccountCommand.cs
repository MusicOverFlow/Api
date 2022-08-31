using System.Security.Cryptography;

namespace Api.Handlers.Commands.AccountCommands;

public class CreateAccountCommand : HandlerBase, Command<Task<Account>, CreateAccountDto>
{
    public CreateAccountCommand(ModelsContext context) : base(context)
    {

    }

    public async Task<Account> Handle(CreateAccountDto createAccount)
    {
        if (!DataValidator.IsMailAddressValid(createAccount.MailAddress))
        {
            throw new HandlerException(ErrorType.InvalidMail);
        }

        if (!DataValidator.IsPasswordValid(createAccount.Password))
        {
            throw new HandlerException(ErrorType.InvalidPassword);
        }

        if (await this.context.Accounts.AnyAsync(a => a.MailAddress.Equals(createAccount.MailAddress)))
        {
            throw new HandlerException(ErrorType.MailAlreadyInUse);
        }

        this.EncryptPassword(createAccount.Password, out byte[] hash, out byte[] salt);

        byte[] fileBytes = null;
        if (createAccount.ProfilPic != null && createAccount.ProfilPic.Length > 0)
        {
            if (!DataValidator.IsImageFormatSupported(createAccount.ProfilPic.FileName))
            {
                throw new HandlerException(ErrorType.WrongFormatFile);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                createAccount.ProfilPic.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
        }

        Account account = new Account()
        {
            MailAddress = createAccount.MailAddress,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = Role.User.ToString(),
            Firstname = createAccount.Firstname ?? "Unknown",
            Lastname = createAccount.Lastname ?? "Unknown",
            Pseudonym = createAccount.Pseudonym ?? "Anonymous",
            PicUrl = Blob.GetProfilPicUrl(fileBytes, createAccount.MailAddress).Result,
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
