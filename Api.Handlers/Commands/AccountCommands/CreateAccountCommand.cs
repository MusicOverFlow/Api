using System.Security.Cryptography;

namespace Api.Handlers.Commands.AccountCommands;

public class CreateAccountCommand : HandlerBase, Command<Task<Account>, CreateAccountDto>
{
    private readonly IContainer container;
    
    public CreateAccountCommand(ModelsContext context, IContainer container) : base(context)
    {
        this.container = container;
    }

    public async Task<Account> Handle(CreateAccountDto message)
    {
        if (!DataValidator.IsMailAddressValid(message.MailAddress))
        {
            throw new HandlerException(ErrorType.InvalidMail);
        }

        if (!DataValidator.IsPasswordValid(message.Password))
        {
            throw new HandlerException(ErrorType.InvalidPassword);
        }

        if (await this.context.Accounts.AnyAsync(a => a.MailAddress.Equals(message.MailAddress)))
        {
            throw new HandlerException(ErrorType.MailAlreadyInUse);
        }

        this.EncryptPassword(message.Password, out byte[] hash, out byte[] salt);

        byte[] fileBytes = null;
        if (message.ProfilPic != null && message.ProfilPic.Length > 0)
        {
            if (!DataValidator.IsImageFormatSupported(message.ProfilPic.FileName))
            {
                throw new HandlerException(ErrorType.WrongFormatFile);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                message.ProfilPic.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
        }

        Account account = new Account()
        {
            MailAddress = message.MailAddress,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = Role.User.ToString(),
            Firstname = message.Firstname ?? "Unknown",
            Lastname = message.Lastname ?? "Unknown",
            Pseudonym = message.Pseudonym ?? "Anonymous",
            PicUrl = this.container.GetProfilPicUrl(fileBytes, message.MailAddress).Result,
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
