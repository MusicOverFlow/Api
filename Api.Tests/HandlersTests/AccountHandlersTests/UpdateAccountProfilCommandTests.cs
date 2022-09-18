namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountProfilCommandTests : TestBase
{
    private Account account;

    public UpdateAccountProfilCommandTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
    }

    [Fact(DisplayName =
       "Updating an account firstname with a firstname\n" +
       "Should update the account's firstname")]
    public async void UpdateAccountProfilHandlerTest_1()
    {
        Account updatedAccount = await new UpdateAccountProfilCommand(this.context).Handle(new UpdateProfilDto()
        {
            MailAddress = this.account.MailAddress,
            Firstname = "MyNewFirstname",
        });
        
        updatedAccount.Firstname.Should().Be("MyNewFirstname");
    }

    [Fact(DisplayName =
       "Updating an account firstname with an empty string\n" +
       "Should not update the account's firstname")]
    public async void UpdateAccountProfilHandlerTest_2()
    {
        Account updatedAccount = await new UpdateAccountProfilCommand(this.context).Handle(new UpdateProfilDto()
        {
            MailAddress = this.account.MailAddress,
            Firstname = string.Empty,
        });
        
        updatedAccount.Firstname.Should().Be("Unknown");
    }

    [Fact(DisplayName =
        "Updating an account firstname without filling it\n" +
        "Should not update the account's firstname")]
    public async void UpdateAccountProfilHandlerTest_3()
    {
        Account updatedAccount = await new UpdateAccountProfilCommand(this.context).Handle(new UpdateProfilDto()
        {
            MailAddress = this.account.MailAddress,
        });
        
        updatedAccount.Firstname.Should().Be("Unknown");
    }

    [Fact(DisplayName =
        "Updating an inexisting account firstname\n" +
        "Should throw exception code 404")]
    public async void UpdateAccountProfilHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountProfilCommand(this.context).Handle(new UpdateProfilDto()
            {
                MailAddress = "unknown@myges.fr",
            }));

        request.Content.StatusCode.Should().Be(404);
    }

    /**
     * Lastname et Pseudonym fonctionnent exactement de la même manière que Firstname
     * => pas besoin d'ajouter des tests pour ces deux propriétés
     */
}
