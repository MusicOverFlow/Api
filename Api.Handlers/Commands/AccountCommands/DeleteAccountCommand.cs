namespace Api.Handlers.Commands.AccountCommands;

public class DeleteAccountCommand : HandlerBase, Command<Task, string>
{
    public DeleteAccountCommand(ModelsContext context) : base(context)
    {

    }

    public async Task Handle(string message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        this.context.Accounts.Remove(account);
        await this.context.SaveChangesAsync();
    }
}
