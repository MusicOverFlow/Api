namespace Api.Handlers.Commands.GroupCommands;

public class JoinGroupCommand : HandlerBase, Command<Task<Group>, JoinGroupDto>
{
    public JoinGroupCommand(ModelsContext context) : base(context)
    {
        
    }

    public async Task<Group> Handle(JoinGroupDto message)
    {
        Account account = await this.context.Accounts
           .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        Group group = await this.context.Groups
            .FirstOrDefaultAsync(p => p.Id.Equals(message.GroupId));

        if (group == null)
        {
            throw new HandlerException(ErrorType.GroupNotFound);
        }

        if (group.Members.Any(m => m.Id.Equals(account.Id)))
        {
            throw new HandlerException(ErrorType.AccountAlreadyInGroup);
        }

        group.Members.Add(account);
        await this.context.SaveChangesAsync();

        return group;
    }
}
