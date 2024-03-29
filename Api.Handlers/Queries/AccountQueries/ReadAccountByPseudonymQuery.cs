﻿namespace Api.Handlers.Queries.AccountQueries;

public class ReadAccountByPseudonymQuery : HandlerBase, Query<Task<List<Account>>, string>
{
    private const int MAX_ACCOUNTS_IN_SEARCHES = 20;

    public ReadAccountByPseudonymQuery(ModelsContext context) : base(context)
    {

    }

    public async Task<List<Account>> Handle(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new HandlerException(ErrorType.InvalidPseudonym);
        }

        List<Account> accounts = new List<Account>();

        await this.context.Accounts
            .ForEachAsync(a =>
            {
                if (accounts.Count >= MAX_ACCOUNTS_IN_SEARCHES)
                {
                    return;
                }

                double pseudonymScore = LevenshteinDistance.Compare(message, a.Pseudonym);

                if (pseudonymScore >= 0.6)
                {
                    accounts.Add(a);
                }
            });

        return accounts;
    }
}
