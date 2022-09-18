namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountMailAddressCommandTests : TestBase
{
    private Account account;

    public UpdateAccountMailAddressCommandTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
    }
    
    [Fact(DisplayName =
        "Updating an account mail address with a valid mail address\n" +
        "Should update the account mail address")]
    public async void UpdateAccountMailAddressHandlerTest_1()
    {
        Account updatedAccount = await new UpdateAccountMailAddressCommand(this.context).Handle(new UpdateMailDto()
        {
            MailAddress = this.account.MailAddress,
            NewMailAddress = "gtnew@myges.fr",
        });
        
        updatedAccount.MailAddress.Should().Be("gtnew@myges.fr");
    }

    [Fact(DisplayName =
        "Updating an account mail address with an invalid mail address\n" +
        "Should throw exception code 400")]
    public async void UpdateAccountMailAddressHandlerTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountMailAddressCommand(this.context).Handle(new UpdateMailDto()
            {
                MailAddress = this.account.MailAddress,
                NewMailAddress = "hello",
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Updating an account mail address with an already registered mail address\n" +
        "Should throw exception code 400")]
    public async void UpdateAccountMailAddressHandlerTest_3()
    {
        await this.RegisterNewAccount("gtnew@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountMailAddressCommand(this.context).Handle(new UpdateMailDto()
            {
                MailAddress = this.account.MailAddress,
                NewMailAddress = "gtnew@myges.fr",
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Updating an inexisting account mail address\n" +
        "Should throw exception with 404")]
    public async void UpdateAccountMailAddressHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountMailAddressCommand(this.context).Handle(new UpdateMailDto()
            {
                MailAddress = "myMail@myges.fr",
                NewMailAddress = "myNewMail@myges.fr",
            }));

        request.Content.StatusCode.Should().Be(404);
    }
}
