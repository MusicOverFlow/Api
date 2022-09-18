namespace Api.Handlers.Commands.GroupCommands;

public class LeaveGroupCommand : HandlerBase, Command<Task, LeaveGroupDto>
{
    public LeaveGroupCommand(ModelsContext context) : base(context)
    {
        
    }

    public async Task Handle(LeaveGroupDto message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        Group group = await this.context.Groups
            .FirstOrDefaultAsync(g => g.Id.Equals(message.GroupId));

        if (group == null)
        {
            throw new HandlerException(ErrorType.GroupNotFound);
        }

        if (group.Owner.Equals(account))
        {
            throw new HandlerException(ErrorType.LeaveWhileOwner);
        }

        group.Members.Remove(account);
        account.Groups.Remove(group);

        await this.context.SaveChangesAsync();
    }
}
