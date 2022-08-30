using Api.Handlers.Kernel;
using Api.Models.ExpositionModels.Requests;

namespace Api.Handlers.Commands;

public class FollowAccountCommand : HandlerBase, Command<Task, FollowRequest>
{
    public FollowAccountCommand(ModelsContext context) : base(context)
    {

    }

    public async Task Handle(FollowRequest followRequest)
    {
        Account callerAccount = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(followRequest.CallerMail));

        Account targetAccount = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(followRequest.TargetMail));

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
