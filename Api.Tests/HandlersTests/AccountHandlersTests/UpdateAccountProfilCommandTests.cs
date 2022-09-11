namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountProfilCommandTests : TestBase
{
    [Fact(DisplayName =
       "Updating an account firstname with a firstname\n" +
       "Should update the account's firstname")]
    public async void UpdateAccountProfilHandlerTest_1()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        Account updatedAccount = await new UpdateAccountProfilCommand(this.context).Handle(new UpdateProfilDto()
        {
            MailAddress = "gt@myges.fr",
            Firstname = "MyNewFirstname",
        });
        
        updatedAccount.Firstname.Should().Be("MyNewFirstname");
    }

    [Fact(DisplayName =
       "Updating an account firstname with an empty string\n" +
       "Should not update the account's firstname")]
    public async void UpdateAccountProfilHandlerTest_2()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        Account updatedAccount = await new UpdateAccountProfilCommand(this.context).Handle(new UpdateProfilDto()
        {
            MailAddress = "gt@myges.fr",
            Firstname = string.Empty,
        });
        
        updatedAccount.Firstname.Should().Be("Unknown");
    }

    [Fact(DisplayName =
        "Updating an account firstname without filling it\n" +
        "Should not update the account's firstname")]
    public async void UpdateAccountProfilHandlerTest_3()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        Account updatedAccount = await new UpdateAccountProfilCommand(this.context).Handle(new UpdateProfilDto()
        {
            MailAddress = "gt@myges.fr",
        });
        
        updatedAccount.Firstname.Should().Be("Unknown");
    }

    [Fact(DisplayName =
        "Updating an inexisting account firstname\n" +
        "Should throw exception with code 404 and error type \"Compte introuvable\"")]
    public async void UpdateAccountProfilHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountProfilCommand(this.context).Handle(new UpdateProfilDto()
            {
                MailAddress = "gt@myges.fr",
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Compte introuvable");
    }

    /**
     * Lastname et Pseudonym fonctionnent exactement de la même manière que Firstname
     * => pas besoin d'ajouter des tests pour ces deux propriétés
     */
}
