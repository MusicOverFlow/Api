namespace Api.Handlers.Queries.AccountQueries;

public class ReadAccountByMailQuery : HandlerBase, Query<Task<List<Account>>, string>
{
    public ReadAccountByMailQuery(ModelsContext context) : base(context)
    {

    }

    public async Task<List<Account>> Handle(string message = null)
    {
        IQueryable<Account> query = this.context.Accounts;

        if (!string.IsNullOrWhiteSpace(message))
        {
            query = query.Where(a => a.MailAddress.Equals(message));
        }

        List<Account> accounts = await query.ToListAsync();

        if (!string.IsNullOrWhiteSpace(message) && accounts.Count == 0)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        return accounts;
    }
}