using Api.Handlers.Kernel;

namespace Api.Handlers.Commands;

public class DeleteAccountCommand : HandlerBase, Command<Task, string>
{
    public DeleteAccountCommand(ModelsContext context) : base(context)
    {

    }

    public async Task Handle(string mailAddress)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        this.context.Accounts.Remove(account);
        await this.context.SaveChangesAsync();
    }
}
