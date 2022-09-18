namespace Api.Handlers.Commands.GroupCommands;

public class KickMemberFromGroupCommand : HandlerBase, Command<Task<Group>, KickMemberDto>
{
    public KickMemberFromGroupCommand(ModelsContext context) : base(context)
    {
        
    }

    public async Task<Group> Handle(KickMemberDto message)
    {
        Account callerAccount = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.CallerMailAddress));

        if (callerAccount == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        Group group = await this.context.Groups
            .FirstOrDefaultAsync(p => p.Id.Equals(message.GroupId));

        if (group == null)
        {
            throw new HandlerException(ErrorType.GroupNotFound);
        }

        if (!group.Owner.Id.Equals(callerAccount.Id))
        {
            throw new HandlerException(ErrorType.NotOwnerOfGroup);
        }

        Account memberAccount = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.MemberMailAddress));

        if (memberAccount == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        if (callerAccount.Equals(memberAccount))
        {
            throw new HandlerException(ErrorType.LeaveWhileOwner);
        }

        if (!group.Members.Any(m => m.MailAddress.Equals(message.MemberMailAddress)))
        {
            throw new HandlerException(ErrorType.Undefined);
        }

        group.Members.Remove(memberAccount);
        await this.context.SaveChangesAsync();

        return group;
    }
}
