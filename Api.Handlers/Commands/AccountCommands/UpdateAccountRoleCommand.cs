namespace Api.Handlers.Commands.AccountCommands;

public class UpdateAccountRoleCommand : HandlerBase, Command<Task, UpdateAccountRoleDto>
{
    public UpdateAccountRoleCommand(ModelsContext context) : base(context)
    {
        
    }
    
    public async Task Handle(UpdateAccountRoleDto updateRole)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(updateRole.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        string newRole = RoleParser.Handle(updateRole.Role);

        if (string.IsNullOrWhiteSpace(newRole))
        {
            throw new HandlerException(ErrorType.InvalidRole);
        }

        account.Role = newRole;
        await this.context.SaveChangesAsync();
    }
}