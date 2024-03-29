﻿using Api.Handlers.Queries.AccountQueries;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult> Read(string mailAddress = null)
    {
        try
        {
            List<Account> accounts = await this.handlers.Get<ReadAccountByMailQuery>().Handle(mailAddress);

            return Ok(accounts
                .Select(a => Mapper.AccountToResource(a))
                .ToList());
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
