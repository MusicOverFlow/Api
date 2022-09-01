namespace Api.Handlers.Queries.AccountQueries;

public class ReadAccountsByNameQuery : HandlerBase, Query<Task<List<Account>>, ReadByNamesDto>
{
    private const int MAX_ACCOUNTS_IN_SEARCHES = 20;

    public ReadAccountsByNameQuery(ModelsContext context) : base(context)
    {

    }

    public async Task<List<Account>> Handle(ReadByNamesDto message)
    {
        if (string.IsNullOrWhiteSpace(message.Firstname) && string.IsNullOrWhiteSpace(message.Lastname))
        {
            throw new HandlerException(ErrorType.InvalidName);
        }

        List<Account> accounts = new List<Account>();

        await this.context.Accounts
            .Include(a => a.Follows)
            .ForEachAsync(a =>
            {
                if (accounts.Count >= MAX_ACCOUNTS_IN_SEARCHES)
                {
                    return;
                }

                double lastnameScore = LevenshteinDistance.Compare(message.Lastname, a.Lastname);

                if (lastnameScore >= 0.6)
                {
                    accounts.Add(a);
                }
                else if (!string.IsNullOrWhiteSpace(message.Firstname))
                {
                    double firstnameScore = LevenshteinDistance.Compare(message.Firstname, a.Firstname);

                    if (lastnameScore + firstnameScore >= 1.1)
                    {
                        accounts.Add(a);
                    }
                }
            });

        return accounts;
    }
}
