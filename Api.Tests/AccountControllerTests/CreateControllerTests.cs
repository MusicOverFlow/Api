using Api.Models.ExpositionModels.Resources;

namespace Api.Tests.AccountControllerTests;

public class CreateControllerTests : TestBase
{
    /*
    [Fact(DisplayName =
        "Account creation with valid request\n" +
        "Should return CreatedResult with status code 201")]
    public async void AccountCreation_1()
    {
        var request = await base.accountsController.Create(
                mailAddress: "gtouchet@myges.fr",
                password: "123Pass!",
                firstname: "Guillaume",
                lastname: "Touchet",
                pseudonym: null,
                profilPic: null);

        request.Result.Should().BeOfType<CreatedResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact(DisplayName =
        "Account creation with request missing names\n" +
        "Should return CreatedResult with status code 201")]
    public async void AccountCreation_2()
    {
        var request = await base.accountsController.Create(
                mailAddress: "gtouchet@myges.fr",
                password: "123Pass!",
                firstname: null,
                lastname: null,
                pseudonym: null,
                profilPic: null);

        request.Result.Should().BeOfType<CreatedResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact(DisplayName =
        "Account creation with request missing names\n" +
        "Should set default names")]
    public async void AccountCreation_3()
    {
        var request = await base.accountsController.Create(
                mailAddress: "gtouchet@myges.fr",
                password: "123Pass!",
                firstname: null,
                lastname: null,
                pseudonym: null,
                profilPic: null);
        var result = request.Result as CreatedResult;
        var account = result.Value as AccountResource;

        account.Firstname.Should().Be("Unknown");
        account.Lastname.Should().Be("Unknown");
    }

    [Fact(DisplayName =
        "Account creation with request missing mail address\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void AccountCreation_4()
    {
        var request = await base.accountsController.Create(
                mailAddress: null,
                password: "123Pass!",
                firstname: "Guillaume",
                lastname: "Touchet",
                pseudonym: null,
                profilPic: null);

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Account creation with request missing password\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void AccountCreation_5()
    {
        var request = await base.accountsController.Create(
                mailAddress: "gtouchet@myges.fr",
                password: null,
                firstname: "Guillaume",
                lastname: "Touchet",
                pseudonym: null,
                profilPic: null);

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Account creation with request with invalid mail address\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void AccountCreation_6()
    {
        var request = await base.accountsController.Create(
                mailAddress: "myMailAddress",
                password: "123Pass!",
                firstname: "Guillaume",
                lastname: "Touchet",
                pseudonym: null,
                profilPic: null);

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Account creation with request with invalid password\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void AccountCreation_7()
    {
        var request = await base.accountsController.Create(
                mailAddress: "gtouchet@myges.fr",
                password: "123",
                firstname: "Guillaume",
                lastname: "Touchet",
                pseudonym: null,
                profilPic: null);

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Account creation result should be exposed as AccountResource")]
    public async void AccountCreation_DataMapping()
    {
        var request = await base.accountsController.Create(
                mailAddress: "gtouchet@myges.fr",
                password: "123Pass!",
                firstname: "Guillaume",
                lastname: "Touchet",
                pseudonym: null,
                profilPic: null);
        var result = request.Result as CreatedResult;

        result.Value.Should().BeOfType<AccountResource>();
    }
    */
}
