using Api.Controllers;
using Api.ExpositionModels;
using Api.Models;
using Api.Utilitaries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Api.Tests.AccountsController_Tests;

public class AccountsControllerTestsBase
{
    protected readonly AccountsController accountsController = new AccountsController(
            new ModelsContext(new DbContextOptionsBuilder<ModelsContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options),
            new DataValidator(),
            new Mapper(),
            new Utilitaries.StringComparer());

    protected async Task<ActionResult<AccountResource>> CreateAccount(string mailAddress, string password, string firstname, string lastname)
    {
        return await this.accountsController.Create(new CreateAccountRequest()
        {
            MailAddress = mailAddress,
            Password = password,
            Firstname = firstname,
            Lastname = lastname,
        });
    }
}
