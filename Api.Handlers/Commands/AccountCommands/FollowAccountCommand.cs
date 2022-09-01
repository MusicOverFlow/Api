namespace Api.Handlers.Commands.AccountCommands;

public class FollowAccountCommand : HandlerBase, Command<Task, FollowDto>
{
    public FollowAccountCommand(ModelsContext context) : base(context)
    {

    }

    public async Task Handle(FollowDto message)
    {
        Account callerAccount = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.CallerMail));

        Account targetAccount = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.TargetMail));

        if (callerAccount == null || targetAccount == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        if (callerAccount.Equals(targetAccount))
        {
            throw new HandlerException(ErrorType.SelfFollow);
        }

        if (!callerAccount.Follows.Remove(targetAccount))
        {
            callerAccount.Follows.Add(targetAccount);
        }

        await this.context.SaveChangesAsync();
    }
}
