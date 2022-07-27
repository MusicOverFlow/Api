namespace Api.Tests.AccountControllerTests;

public class ReadControllerTests : TestBase
{
    public ReadControllerTests()
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
        "Read accounts without specifying mail address\n" +
        "Should return OkObjectResult with status code 200")]
    public async void AccountReading_1()
    {
        var request = await base.accountsController.Read();

        request.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact(DisplayName =
        "Read accounts without specifying mail address\n" +
        "Should return all accounts")]
    public async void AccountReading_2()
    {
        await base.accountsController.Create(
            mailAddress: "gtouchet2@myges.fr",
            password: "123Pass!",
            firstname: null,
            lastname: null,
            pseudonym: null,
            profilPic: null);
        var request = await base.accountsController.Read();
        var result = request.Result as OkObjectResult;
        
        result.Value.As<List<AccountResource_WithPosts_AndGroups_AndFollows>>().Count.Should().Be(2);
    }

    [Fact(DisplayName =
        "Read accounts specifying a mail address\n" +
        "Should return OkObjectResult with status code 200")]
    public async void AccountReading_3()
    {
        var request = await base.accountsController.Read("gtouchet@myges.fr");

        request.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact(DisplayName =
        "Read accounts specifying a mail address\n" +
        "Should return the account resources")]
    public async void AccountReading_4()
    {
        var request = await base.accountsController.Read("gtouchet@myges.fr");
        var result = request.Result as OkObjectResult;
        var account = result.Value as List<AccountResource_WithPosts_AndGroups_AndFollows>;

        account.First().MailAddress.Should().Be("gtouchet@myges.fr");
        account.First().Firstname.Should().Be("Guillaume");
        account.First().Lastname.Should().Be("Touchet");
    }

    [Fact(DisplayName =
        "Read accounts specifying a random mail address\n" +
        "Should return NotFoundObjectResult with status code 404")]
    public async void AccountReading_5()
    {
        var request = await base.accountsController.Read("random@whatever.fr");

        request.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact(DisplayName =
        "Account reading results should be exposed as AccountResource_WithPosts_AndGroups")]
    public async void AccountCreation_DataMapping()
    {
        var request = await base.accountsController.Read();
        var result = request.Result as OkObjectResult;

        result.Value.Should().BeOfType<List<AccountResource_WithPosts_AndGroups_AndFollows>>();
    }
}
