namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class DeleteAccountCommandTests : TestBase
{
    private Account account;
    
    public DeleteAccountCommandTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
    }

    [Fact(DisplayName =
        "Deleting an existing account\n" +
        "Should delete the account")]
    public async void DeleteAccountHandlerTest_1()
    {
        await new DeleteAccountCommand(this.context).Handle(this.account.MailAddress);

        Account deletedAccount = this.context.Accounts.FirstOrDefault(a => a.MailAddress.Equals(this.account.MailAddress));
        deletedAccount.Should().BeNull();
    }

    [Fact(DisplayName =
        "Deleting an unexisting account\n" +
        "Should throw exception code 404")]
    public async void DeleteAccountHandlerTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new DeleteAccountCommand(this.context).Handle("nonExistingAccount@myges.fr"));

        request.Content.StatusCode.Should().Be(404);
    }
}
