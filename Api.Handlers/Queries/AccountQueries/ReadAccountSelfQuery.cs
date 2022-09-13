namespace Api.Handlers.Queries.AccountQueries;

public class ReadAccountSelfQuery : HandlerBase, Query<Task<Account>, string>
{
    public ReadAccountSelfQuery(ModelsContext context) : base(context)
    {
        
    }

    public async Task<Account> Handle(string message)
    {
        Account account = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(message))
            .FirstOrDefaultAsync();

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        return account;
    }
}
