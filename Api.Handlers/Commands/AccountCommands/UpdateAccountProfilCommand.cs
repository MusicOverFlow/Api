namespace Api.Handlers.Commands.AccountCommands;

public class UpdateAccountProfilCommand : HandlerBase, Command<Task, UpdateProfilDto>
{
    public UpdateAccountProfilCommand(ModelsContext context) : base(context)
    {
        
    }

    public async Task Handle(UpdateProfilDto updateProfil)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(updateProfil.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        account.Firstname = !string.IsNullOrWhiteSpace(updateProfil.Firstname) ? updateProfil.Firstname : account.Firstname;
        account.Lastname = !string.IsNullOrWhiteSpace(updateProfil.Lastname) ? updateProfil.Lastname : account.Lastname;
        account.Pseudonym = !string.IsNullOrWhiteSpace(updateProfil.Pseudonym) ? updateProfil.Pseudonym : account.Pseudonym;
        await this.context.SaveChangesAsync();
    }
}
