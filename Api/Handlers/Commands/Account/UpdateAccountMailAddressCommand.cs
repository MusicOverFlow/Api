using Api.Handlers.Dtos;
using Api.Handlers.Kernel;

namespace Api.Handlers.Commands;

public class UpdateAccountMailAddressCommand : HandlerBase, Command<Task, UpdateMailDto>
{
    public UpdateAccountMailAddressCommand(ModelsContext context) : base(context)
    {
        
    }

    public async Task Handle(UpdateMailDto updateMail)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(updateMail.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        if (!DataValidator.IsMailAddressValid(updateMail.NewMailAddress))
        {
            throw new HandlerException(ErrorType.InvalidMail);
        }

        if (await this.context.Accounts.AnyAsync(a => a.MailAddress.Equals(updateMail.NewMailAddress)))
        {
            throw new HandlerException(ErrorType.MailAlreadyInUse);
        }

        account.MailAddress = updateMail.NewMailAddress;
        await this.context.SaveChangesAsync();
    }
}
