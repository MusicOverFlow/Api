using Api.Handlers.Kernel;
using Api.Models.ExpositionModels.Requests;

namespace Api.Handlers.Queries;

public class ReadAccountsByNameQuery : HandlerBase, Query<Task<List<Account>>, ReadByNamesRequest>
{
    private const int MAX_ACCOUNTS_IN_SEARCHES = 20;

    public ReadAccountsByNameQuery(ModelsContext context) : base(context)
    {

    }

    public async Task<List<Account>> Handle(ReadByNamesRequest namesRequest)
    {
        if (string.IsNullOrWhiteSpace(namesRequest.Firstname) && string.IsNullOrWhiteSpace(namesRequest.Lastname))
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

                double lastnameScore = LevenshteinDistance.Compare(namesRequest.Lastname, a.Lastname);

                if (lastnameScore >= 0.6)
                {
                    accounts.Add(a);
                }
                else if (!string.IsNullOrWhiteSpace(namesRequest.Firstname))
                {
                    double firstnameScore = LevenshteinDistance.Compare(namesRequest.Firstname, a.Firstname);

                    if ((lastnameScore + firstnameScore) >= 1.1)
                    {
                        accounts.Add(a);
                    }
                }
            });

        return accounts;
    }
}
