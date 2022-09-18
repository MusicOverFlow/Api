namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountPasswordCommandTests : TestBase
{
    private Account account;
    
    public UpdateAccountPasswordCommandTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
    }
    
    [Fact(DisplayName =
        "Updating an account password with a valid password\n" +
        "Should update the account's password")]
    public async void UpdateAccountPasswordHandlerTest_1()
    {
        byte[] beforeUpdateHash = account.PasswordHash;
        
        Account updatedAccount = await new UpdateAccountPasswordCommand(this.context).Handle(new UpdatePasswordDto()
        {
            MailAddress = this.account.MailAddress,
            NewPassword = "456drowssaP?",
        });

        updatedAccount.PasswordHash.Should().NotBeEquivalentTo(beforeUpdateHash);
    }

    [Fact(DisplayName =
        "Updating an account password with an invalid password\n" +
        "Should throw exception code 400")]
    public async void UpdateAccountPasswordHandlerTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountPasswordCommand(this.context).Handle(new UpdatePasswordDto()
            {
                MailAddress = this.account.MailAddress,
                NewPassword = "myPass",
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Updating an account password with an invalid password\n" +
        "Should not update the account's password")]
    public async void UpdateAccountPasswordHandlerTest_3()
    {
        Account updatedAccount = this.account;
        try
        {
            updatedAccount = await new UpdateAccountPasswordCommand(this.context).Handle(new UpdatePasswordDto()
            {
                MailAddress = this.account.MailAddress,
                NewPassword = "myPass",
            });
        }
        catch (HandlerException)
        {

        }
        
        updatedAccount.PasswordHash.Should().Equal(this.account.PasswordHash);
    }

    [Fact(DisplayName =
        "Updating an inexisting account password\n" +
        "Should throw exception code 404")]
    public async void UpdateAccountPasswordHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountPasswordCommand(this.context).Handle(new UpdatePasswordDto()
            {
                MailAddress = "unknown@myges.fr",
                NewPassword = "123Password!",
            }));

        request.Content.StatusCode.Should().Be(404);
    }
}
