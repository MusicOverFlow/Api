namespace Api.Handlers.Commands.AccountCommands;

public class UpdateAccountProfilCommand : HandlerBase, Command<Task<Account>, UpdateProfilDto>
{
    public UpdateAccountProfilCommand(ModelsContext context) : base(context)
    {
        
    }

    public async Task<Account> Handle(UpdateProfilDto message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        account.Firstname = !string.IsNullOrWhiteSpace(message.Firstname) ? message.Firstname : account.Firstname;
        account.Lastname = !string.IsNullOrWhiteSpace(message.Lastname) ? message.Lastname : account.Lastname;
        account.Pseudonym = !string.IsNullOrWhiteSpace(message.Pseudonym) ? message.Pseudonym : account.Pseudonym;
        await this.context.SaveChangesAsync();

        return account;
    }
}
