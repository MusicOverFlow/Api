using Api.Handlers.Kernel;
using Api.Handlers.Utilitaries;
using Api.Models;
using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Handlers.Queries.AccountQueries;

public class ReadAccountByPseudonymQuery : HandlerBase, Query<Task<List<Account>>, string>
{
    private const int MAX_ACCOUNTS_IN_SEARCHES = 20;

    public ReadAccountByPseudonymQuery(ModelsContext context) : base(context)
    {

    }

    public async Task<List<Account>> Handle(string pseudonym)
    {
        if (string.IsNullOrWhiteSpace(pseudonym))
        {
            throw new HandlerException(ErrorType.InvalidPseudonym);
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

                double pseudonymScore = LevenshteinDistance.Compare(pseudonym, a.Pseudonym);

                if (pseudonymScore >= 0.6)
                {
                    accounts.Add(a);
                }
            });

        return accounts;
    }
}
