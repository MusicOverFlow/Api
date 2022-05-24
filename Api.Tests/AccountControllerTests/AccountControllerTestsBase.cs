using Api.Controllers.AccountControllers;
using Api.Models;
using Api.Utilitaries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Api.Tests.AccountControllerTests;

public class AccountControllerTestsBase
{
    protected readonly AccountController accountsController = new AccountController(
            new ModelsContext(new DbContextOptionsBuilder<ModelsContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options),
            new Mapper(),
            new DataValidator(),
            new Utilitaries.StringComparer());

    protected async Task<ActionResult<AccountResource>> CreateAccount(string mailAddress, string password, string firstname, string lastname)
    {
        return await accountsController.Create(new CreateAccountRequest()
        {
            MailAddress = mailAddress,
            Password = password,
            Firstname = firstname,
            Lastname = lastname,
        });
    }
}
