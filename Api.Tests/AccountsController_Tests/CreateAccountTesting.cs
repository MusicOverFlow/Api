using Api.Models.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Api.Tests.AccountsController_Tests;

public class CreateAccountTesting : AccountsControllerTestsBase
{
    // TODO: rewrite tests
    [Fact(DisplayName =
        "")]
    public async void CreateAccount_ValidAccount_ShouldReturn_Created()
    {
        ActionResult<AccountResource> request = await base.CreateAccount("gtouchet@myges.fr", "123Pass!", "Guillaume", "Touchet");

        request.Result.GetType().Should().Be(typeof(CreatedResult));
    }

    [Fact(DisplayName =
        "")]
    public async void CreateAccount_WithoutName_ShouldReturn_Created()
    {
        ActionResult<AccountResource> request = await base.CreateAccount("gtouchet@myges.fr", "123Pass!", null, null);

        request.Result.GetType().Should().Be(typeof(CreatedResult));
    }

    [Fact(DisplayName =
        "")]
    public async void CreateAccount_WithoutName_ShouldSetDefaultNames()
    {
        ActionResult<AccountResource> request = await base.CreateAccount("gtouchet@myges.fr", "123Pass!", null, null);
        CreatedResult result = request.Result as CreatedResult;
        AccountResource account = result.Value as AccountResource;

        account.Firstname.Should().Be("Unknown");
        account.Lastname.Should().Be("Unknown");
    }

    [Fact(DisplayName =
        "")]
    public async void CreateAccount_WithoutMailAddress_ShouldReturn_BadRequest()
    {
        ActionResult<AccountResource> request = await base.CreateAccount(null, "123Pass!", "Guillaume", "Touchet");

        request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
    }

    [Fact(DisplayName =
        "")]
    public async void CreateAccount_WithoutPassword_ShouldReturn_BadRequest()
    {
        ActionResult<AccountResource> request = await base.CreateAccount("gtouchet@myges.fr", null, "Guillaume", "Touchet");

        request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
    }

    [Fact(DisplayName =
        "")]
    public async void CreateTwoAccounts_WithSameMailAddresses_ShouldReturn_BadRequest()
    {
        await base.CreateAccount("gtouchet@myges.fr", "123Pass!", "Guillaume", "Touchet");

        ActionResult<AccountResource> request = await base.CreateAccount("gtouchet@myges.fr", "123Pass!", "Guillaume", "Touchet");

        request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
    }
}
