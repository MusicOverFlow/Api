namespace Api.Handlers.Commands.AccountCommands;

public class UpdateAccountProfilPicCommand : HandlerBase, Command<Task<Account>, UpdateProfilPicDto>
{
    private readonly IContainer container;

    public UpdateAccountProfilPicCommand(ModelsContext context, IContainer container) : base(context)
    {
        this.container = container;
    }

    public async Task<Account> Handle(UpdateProfilPicDto message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.MailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        if (message.ProfilPic == null || message.ProfilPic.Length == 0 ||
            !DataValidator.IsImageFormatSupported(message.ProfilPic.FileName))
        {
            throw new HandlerException(ErrorType.WrongFormatFile);
        }

        using (var ms = new MemoryStream())
        {
            message.ProfilPic.CopyTo(ms);
            account.PicUrl = this.container.GetProfilPicUrl(ms.ToArray(), account.MailAddress).Result;
        }

        await this.context.SaveChangesAsync();

        return account;
    }
}