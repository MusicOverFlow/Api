namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class ReadAccountSelfQueryTests : TestBase
{
    private Account account;

    public ReadAccountSelfQueryTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
    }

    [Fact(DisplayName = "Reading self should return the account")]
    public async void ReadAccountSelfQueryTest_1()
    {
        Account self = await new ReadAccountSelfQuery(this.context).Handle(this.account.MailAddress);
        self.Should().BeEquivalentTo(this.account);
    }

    [Fact(DisplayName = "Reading an inexisting account should return an error code 404")]
    public async void ReadAccountSelfQueryTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new ReadAccountSelfQuery(this.context).Handle("unknown@myges.fr"));

        request.Content.StatusCode.Should().Be(404);
    }
}
