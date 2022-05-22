using Api.Models.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;

namespace Api.Tests.AccountsController_Tests;

public class ReadAccountTesting : AccountsControllerTestsBase
{
    // TODO: rewrite tests
    public ReadAccountTesting()
    {
        this.CreateTestAccount();
    }

    private async void CreateTestAccount()
    {
        await base.CreateAccount("gtouchet@myges.fr", "123Pass!", "Guillaume", "Touchet");
    }

    [Fact]
    public async void ReadAllAccounts_Size_ShouldBe_1()
    {
        ActionResult<List<AccountResource>> request = await accountsController.Read();
        OkObjectResult result = request.Result as OkObjectResult;
        List<AccountResource> accounts = result.Value as List<AccountResource>;

        int size = 0;
        accounts.ForEach(account => size += 1);

        size.Should().Be(1);
    }

    [Fact]
    public async void ReadAccount_ExistingAddress_ShouldReturn_Ok()
    {
        ActionResult<List<AccountResource>> request = await accountsController.Read("gtouchet@myges.fr");

        request.Result.GetType().Should().Be(typeof(OkObjectResult));
    }

    [Fact]
    public async void ReadAccount_RandomAddress_ShouldReturn_NotFound()
    {
        ActionResult<List<AccountResource>> request = await accountsController.Read("random@whatever.fr");

        request.Result.GetType().Should().Be(typeof(NotFoundObjectResult));
    }
}
