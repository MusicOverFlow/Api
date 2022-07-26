namespace Api.Tests.AccountControllerTests;

public class ReadNameControllerTests : TestBase
{
    public ReadNameControllerTests()
    {
        _ = base.accountsController.Create(
                mailAddress: "gtouchet@myges.fr",
                password: "123Pass!",
                firstname: "Guillaume",
                lastname: "Touchet",
                pseudonym: null,
                profilPic: null);
    }

    [Fact(DisplayName =
        "Searching lastname 'Touch'\n" +
        "Should return the account\n" +
        "Because similarity score is high enough")]
    public async void ReadAccountByName_1()
    {
        var request = await base.accountsController.ReadName(new ReadByNames()
        {
            Lastname = "Touch",
        });
        var result = request.Result as OkObjectResult;

        result.Value.As<List<AccountResource>>().First().Lastname.Should().Be("Touchet");
    }

    [Fact(DisplayName =
        "Searching lastname 'oucet'\n" +
        "Should return the account\n" +
        "Because similarity score is high enough")]
    public async void ReadAccountByName_2()
    {
        var request = await base.accountsController.ReadName(new ReadByNames()
        {
            Lastname = "oucet",
        });
        var result = request.Result as OkObjectResult;

        result.Value.As<List<AccountResource>>().First().Lastname.Should().Be("Touchet");
    }

    [Fact(DisplayName =
        "Searching lastname 'Tou'\n" +
        "Should not return the account\n" +
        "Because similarity score is too low")]
    public async void ReadAccountByName_3()
    {
        var request = await base.accountsController.ReadName(new ReadByNames()
        {
            Lastname = "Tou",
        });
        var result = request.Result as OkObjectResult;

        result.Value.As<List<AccountResource>>().Count.Should().Be(0);
    }

    [Fact(DisplayName =
        "Searching lastname 'Tou' and firstname 'Guillau'\n" +
        "Should return the account\n" +
        "Because names added similarity score are high enough")]
    public async void ReadAccountByName_4()
    {
        var request = await base.accountsController.ReadName(new ReadByNames()
        {
            Firstname = "Guillau",
            Lastname = "Tou",
        });
        var result = request.Result as OkObjectResult;

        result.Value.As<List<AccountResource>>().First().Lastname.Should().Be("Touchet");
    }

    [Fact(DisplayName =
        "Searching lastname 'Touchai' and firstname 'Gillome'\n" +
        "Should return the account\n" +
        "Because names added similarity score are high enough")]
    public async void ReadAccountByName_5()
    {
        var request = await base.accountsController.ReadName(new ReadByNames()
        {
            Firstname = "Gillome",
            Lastname = "Touchai",
        });
        var result = request.Result as OkObjectResult;

        result.Value.As<List<AccountResource>>().First().Lastname.Should().Be("Touchet");
    }
}
