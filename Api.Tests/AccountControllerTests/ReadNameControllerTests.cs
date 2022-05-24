﻿using System.Linq;

namespace Api.Tests.AccountControllerTests;

public class ReadNameControllerTests : AccountControllerTestsBase
{
    public ReadNameControllerTests()
    {
        _ = CreateAccount("gtouchet@myges.fr", "123Pass!", "Guillaume", "Touchet");
    }

    [Fact(DisplayName =
        "Searching lastname 'Touch'\n" +
        "Should return the account\n" +
        "Because similarity score is high enough")]
    public async void ReadAccountByName_1()
    {
        ActionResult<List<AccountResource>> request = await accountsController.ReadName(new ReadByNames()
        {
            Lastname = "Touch",
        });
        OkObjectResult result = request.Result as OkObjectResult;
        List<AccountResource> accounts = result.Value as List<AccountResource>;

        accounts.First().Lastname.Should().Be("Touchet");
    }

    [Fact(DisplayName =
        "Searching lastname 'oucet'\n" +
        "Should return the account\n" +
        "Because similarity score is high enough")]
    public async void ReadAccountByName_2()
    {
        ActionResult<List<AccountResource>> request = await accountsController.ReadName(new ReadByNames()
        {
            Lastname = "oucet",
        });
        OkObjectResult result = request.Result as OkObjectResult;
        List<AccountResource> accounts = result.Value as List<AccountResource>;

        accounts.First().Firstname.Should().Be("Guillaume");
        accounts.First().Lastname.Should().Be("Touchet");
    }

    [Fact(DisplayName =
        "Searching lastname 'Tou'\n" +
        "Should not return the account\n" +
        "Because similarity score is too low")]
    public async void ReadAccountByName_3()
    {
        ActionResult<List<AccountResource>> request = await accountsController.ReadName(new ReadByNames()
        {
            Lastname = "Tou",
        });
        OkObjectResult result = request.Result as OkObjectResult;
        List<AccountResource> accounts = result.Value as List<AccountResource>;

        accounts.Count.Should().Be(0);
    }

    [Fact(DisplayName =
        "Searching lastname 'Tou' and firstname 'Guillau'\n" +
        "Should return the account\n" +
        "Because names added similarity score are high enough")]
    public async void ReadAccountByName_4()
    {
        ActionResult<List<AccountResource>> request = await accountsController.ReadName(new ReadByNames()
        {
            Firstname = "Guillau",
            Lastname = "Tou",
        });
        OkObjectResult result = request.Result as OkObjectResult;
        List<AccountResource> accounts = result.Value as List<AccountResource>;

        accounts.First().Firstname.Should().Be("Guillaume");
        accounts.First().Lastname.Should().Be("Touchet");
    }

    [Fact(DisplayName =
        "Searching lastname 'Touchai' and firstname 'Gillome'\n" +
        "Should return the account\n" +
        "Because names added similarity score are high enough")]
    public async void ReadAccountByName_5()
    {
        ActionResult<List<AccountResource>> request = await accountsController.ReadName(new ReadByNames()
        {
            Firstname = "Gillome",
            Lastname = "Touchai",
        });
        OkObjectResult result = request.Result as OkObjectResult;
        List<AccountResource> accounts = result.Value as List<AccountResource>;

        accounts.First().Firstname.Should().Be("Guillaume");
        accounts.First().Lastname.Should().Be("Touchet");
    }
}