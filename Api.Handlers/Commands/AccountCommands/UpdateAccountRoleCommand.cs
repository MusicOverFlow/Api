namespace Api.Handlers.Commands.AccountCommands;

public class UpdateAccountRoleCommand : HandlerBase, Command<Task<Account>, UpdateAccountRoleDto>
{
    public UpdateAccountRoleCommand(ModelsContext context) : base(context)
    {
        
    }
    
    public async Task<Account> Handle(UpdateAccountRoleDto message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        string newRole = RoleParser.Handle(message.Role);

        if (string.IsNullOrWhiteSpace(newRole))
        {
            throw new HandlerException(ErrorType.InvalidRole);
        }

        account.Role = newRole;
        await this.context.SaveChangesAsync();

        return account;
    }
}