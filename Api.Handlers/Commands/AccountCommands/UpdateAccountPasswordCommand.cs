using System.Security.Cryptography;

namespace Api.Handlers.Commands.AccountCommands;

public class UpdateAccountPasswordCommand : HandlerBase, Command<Task, UpdatePasswordDto>
{
    public UpdateAccountPasswordCommand(ModelsContext context) : base(context)
    {
        
    }

    public async Task Handle(UpdatePasswordDto message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        if (!DataValidator.IsPasswordValid(message.NewPassword))
        {
            throw new HandlerException(ErrorType.InvalidPassword);
        }

        this.EncryptPassword(message.NewPassword, out byte[] hash, out byte[] salt);

        account.PasswordHash = hash;
        account.PasswordSalt = salt;
        
        await this.context.SaveChangesAsync();
    }

    private void EncryptPassword(string password, out byte[] hash, out byte[] salt)
    {
        using HMACSHA512 hmac = new HMACSHA512();
        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        salt = hmac.Key;
    }
}
