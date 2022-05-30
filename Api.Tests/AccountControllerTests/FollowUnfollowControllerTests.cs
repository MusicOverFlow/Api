namespace Api.Tests.AccountControllerTests;

public class FollowUnfollowControllerTests : TestBase
{
    private AccountResource connectedAccount, secondAccount;

    public FollowUnfollowControllerTests()
    {
        this.connectedAccount = this.CreateAccount("gtouchet1@myges.fr", "123Pass!").Result;
        // firstAccount is the caller
        base.MockJwtAuthentication(this.connectedAccount);
        this.secondAccount = this.CreateAccount("gtouchet2@myges.fr", "123Pass!").Result;
    }

    private async Task<AccountResource> CreateAccount(string mail, string password)
    {
        var request = await base.accountsController.Create(new CreateAccountRequest()
        {
            MailAddress = mail,
            Password = password,
        });
        var result = request.Result as CreatedResult;

        return result.Value as AccountResource;
    }

    [Fact(DisplayName =
        "Following an existing account,\n" +
        "Should return OkResult with status code 200")]
    public async void FollowUnfollow_1()
    {
        var request = await base.accountsController.FollowUnfollow(this.secondAccount.MailAddress);

        request.Should().BeOfType<OkResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact(DisplayName =
        "Following an unexisting account,\n" +
        "Should return NotFoundObjectResult with status code 404")]
    public async void FollowUnfollow_2()
    {
        var request = await base.accountsController.FollowUnfollow("myImaginaryFriend@inMyHead.com");

        request.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact(DisplayName =
        "Following your own account,\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void FollowUnfollow_3()
    {
        var request = await base.accountsController.FollowUnfollow(this.connectedAccount.MailAddress);

        request.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Following an existing, not yet followed account,\n" +
        "Should add this other account in connected followed accounts list")]
    public async void FollowUnfollow_4()
    {
        await base.accountsController.FollowUnfollow(this.secondAccount.MailAddress);

        var request = await base.accountsController.Read(this.connectedAccount.MailAddress);
        var result = request.Result as OkObjectResult;
        var accounts = result.Value as List<AccountResource_WithPosts_AndGroups>;
        var account = accounts.First();

        account.Follows.First().Should().BeEquivalentTo(this.secondAccount);
    }

    [Fact(DisplayName =
        "Following an existing, already followed account,\n" +
        "Should remove this other account from connected followed accounts list")]
    public async void FollowUnfollow_5()
    {
        // follow
        await base.accountsController.FollowUnfollow(this.secondAccount.MailAddress);
        // unfollow
        await base.accountsController.FollowUnfollow(this.secondAccount.MailAddress);

        var request = await base.accountsController.Read(this.connectedAccount.MailAddress);
        var result = request.Result as OkObjectResult;
        var accounts = result.Value as List<AccountResource_WithPosts_AndGroups>;
        var account = accounts.First();

        account.Follows.Count.Should().Be(0);
    }
}
