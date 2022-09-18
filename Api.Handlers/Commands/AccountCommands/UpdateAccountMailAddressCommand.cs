namespace Api.Handlers.Commands.AccountCommands;

public class UpdateAccountMailAddressCommand : HandlerBase, Command<Task<Account>, UpdateMailDto>
{
    public UpdateAccountMailAddressCommand(ModelsContext context) : base(context)
    {

    }

    public async Task<Account> Handle(UpdateMailDto message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        if (!DataValidator.IsMailAddressValid(message.NewMailAddress))
        {
            throw new HandlerException(ErrorType.InvalidMail);
        }

        if (await this.context.Accounts.AnyAsync(a => a.MailAddress.Equals(message.NewMailAddress)))
        {
            throw new HandlerException(ErrorType.MailAlreadyInUse);
        }

        account.MailAddress = message.NewMailAddress;
        await this.context.SaveChangesAsync();

        return account;
    }
}
