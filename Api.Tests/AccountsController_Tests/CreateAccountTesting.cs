using Api.ExpositionModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace Api.Tests.AccountsController_Tests;

public class CreateAccountTesting : AccountsControllerTestsBase
{
    [Fact(DisplayName =
        "Account creation with valid request\n" +
        "Should return CreatedResult with status code 201")]
    public async void AccountCreation_1()
    {
        ActionResult<AccountResource> request = await base.CreateAccount("gtouchet@myges.fr", "123Pass!", "Guillaume", "Touchet");

        request.Result.Should().BeOfType<CreatedResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact(DisplayName =
        "Account creation with request missing names\n" +
        "Should return CreatedResult with status code 201")]
    public async void AccountCreation_2()
    {
        ActionResult<AccountResource> request = await base.CreateAccount("gtouchet@myges.fr", "123Pass!", null, null);

        request.Result.Should().BeOfType<CreatedResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact(DisplayName =
        "Account creation with request missing names\n" +
        "Should set default names")]
    public async void AccountCreation_3()
    {
        ActionResult<AccountResource> request = await base.CreateAccount("gtouchet@myges.fr", "123Pass!", null, null);
        CreatedResult result = request.Result as CreatedResult;
        AccountResource account = result.Value as AccountResource;

        account.Firstname.Should().Be("Unknown");
        account.Lastname.Should().Be("Unknown");
    }

    [Fact(DisplayName =
        "Account creation with request missing mail address\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void AccountCreation_4()
    {
        ActionResult<AccountResource> request = await base.CreateAccount(null, "123Pass!", "Guillaume", "Touchet");

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Account creation with request missing password\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void AccountCreation_5()
    {
        ActionResult<AccountResource> request = await base.CreateAccount("gtouchet@myges.fr", null, "Guillaume", "Touchet");

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Account creation with request with invalid mail address\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void AccountCreation_6()
    {
        ActionResult<AccountResource> request = await base.CreateAccount("myMailAddress", "123Pass!", "Guillaume", "Touchet");

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Account creation with request with invalid password\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void AccountCreation_7()
    {
        ActionResult<AccountResource> request = await base.CreateAccount("gtouchet@myges.fr", "123", "Guillaume", "Touchet");

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}
