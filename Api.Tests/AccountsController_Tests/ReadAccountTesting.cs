using Api.ExpositionModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Api.Tests.AccountsController_Tests;

public class ReadAccountTesting : AccountsControllerTestsBase
{
    public ReadAccountTesting()
    {
        _ = base.CreateAccount("gtouchet@myges.fr", "123Pass!", "Guillaume", "Touchet");
    }

    [Fact(DisplayName =
        "Read accounts without specifying mail address\n" +
        "Should return OkObjectResult with status code 200")]
    public async void AccountReading_1()
    {
        await base.CreateAccount("gtouchet2@myges.fr", "123Pass!", "Guillaume", "Touchet");

        ActionResult<List<AccountResource>> request = await base.accountsController.Read();
        OkObjectResult result = request.Result as OkObjectResult;
        List<AccountResource> accounts = result.Value as List<AccountResource>;

        request.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact(DisplayName =
        "Read accounts without specifying mail address\n" +
        "Should return all accounts")]
    public async void AccountReading_2()
    {
        await base.CreateAccount("gtouchet2@myges.fr", "123Pass!", "Guillaume", "Touchet");

        ActionResult<List<AccountResource>> request = await base.accountsController.Read();
        OkObjectResult result = request.Result as OkObjectResult;
        List<AccountResource> accounts = result.Value as List<AccountResource>;

        accounts.Count.Should().Be(2);
    }

    [Fact(DisplayName =
        "Read accounts specifying a mail address\n" +
        "Should return OkObjectResult with status code 200")]
    public async void AccountReading_3()
    {
        ActionResult<List<AccountResource>> request = await base.accountsController.Read("gtouchet@myges.fr");

        request.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact(DisplayName =
        "Read accounts specifying a random mail address\n" +
        "Should return NotFoundObjectResult with status code 404")]
    public async void AccountReading_4()
    {
        ActionResult<List<AccountResource>> request = await base.accountsController.Read("random@whatever.fr");

        request.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
