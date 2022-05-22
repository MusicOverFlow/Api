using Api.Models;
using Api.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using Api.Utilitaries;
using Api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Tests.AccountsController_Tests;

public class AccountsControllerTestsBase
{
    protected readonly AccountsController accountsController;

    public AccountsControllerTestsBase()
    {
        this.accountsController = new AccountsController(
            new ModelsContext(new DbContextOptionsBuilder<ModelsContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options),
            new DataValidator(),
            new Mapper(),
            new Utilitaries.StringComparer());
    }

    protected async Task<ActionResult<AccountResource>> CreateAccount(string mailAddress, string password, string firstname, string lastname)
    {
        return await this.accountsController.Create(new CreateAccount()
        {
            MailAddress = mailAddress,
            Password = password,
            Firstname = firstname,
            Lastname = lastname,
        });
    }
}
