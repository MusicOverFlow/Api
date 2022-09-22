namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class ReadAccountByMailQueryTests : TestBase
{
    private Account account;

    public ReadAccountByMailQueryTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
    }

    [Fact(DisplayName = "Read an account by mail should return the account")]
    public void ReadAccountByMailQueryTest_1()
    {
        Account readAccount = new ReadAccountByMailQuery(this.context).Handle(this.account.MailAddress).Result.First();
        readAccount.Should().BeEquivalentTo(this.account);
    }

    [Fact(DisplayName = "Read an in existing account by mail should return an error code 404")]
    public async void ReadAccountByMailQueryTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new ReadAccountByMailQuery(this.context).Handle("unknown@myges.fr"));

        request.Content.StatusCode.Should().Be(404);
    }
}
