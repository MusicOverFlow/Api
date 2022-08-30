using Api.Handlers.Kernel;

namespace Api.Handlers.Queries
{
    public class ReadAccountByMailQuery : HandlerBase, Query<Task<List<Account>>, string>
    {
        public ReadAccountByMailQuery(ModelsContext context) : base(context)
        {

        }

        public async Task<List<Account>> Handle(string mailAddress = null)
        {
            IQueryable<Account> query = this.context.Accounts;

            if (!string.IsNullOrWhiteSpace(mailAddress))
            {
                query = query.Where(a => a.MailAddress.Equals(mailAddress));
            }

            List<Account> accounts = await query
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(mailAddress) && accounts.Count == 0)
            {
                throw new HandlerException(ErrorType.AccountNotFound);
            }

            return accounts;
        }
    }
}