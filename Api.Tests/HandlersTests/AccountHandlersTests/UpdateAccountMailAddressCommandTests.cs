namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountMailAddressCommandTests : TestBase
{
    [Fact(DisplayName =
        "Updating an account mail address with a valid mail address\n" +
        "Should update the account mail address")]
    public async void UpdateAccountMailAddressHandlerTest_1()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        Account updatedAccount = await new UpdateAccountMailAddressCommand(this.context).Handle(new UpdateMailDto()
        {
            MailAddress = "gt@myges.fr",
            NewMailAddress = "gtnew@myges.fr",
        });
        
        updatedAccount.MailAddress.Should().Be("gtnew@myges.fr");
    }

    [Fact(DisplayName =
        "Updating an account mail address with an invalid mail address\n" +
        "Should throw exception with code 400 and error type \"Adresse mail invalide\"")]
    public async void UpdateAccountMailAddressHandlerTest_2()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountMailAddressCommand(this.context).Handle(new UpdateMailDto()
            {
                MailAddress = "gt@myges.fr",
                NewMailAddress = "hello",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Adresse mail invalide");
    }

    [Fact(DisplayName =
        "Updating an account mail address with an already registered mail address\n" +
        "Should throw exception with code 400 and error type \"Adresse mail déjà enregistrée\"")]
    public async void UpdateAccountMailAddressHandlerTest_3()
    {
        await this.RegisterNewAccount("gtfirst@myges.fr");
        await this.RegisterNewAccount("gtsecond@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountMailAddressCommand(this.context).Handle(new UpdateMailDto()
            {
                MailAddress = "gtfirst@myges.fr",
                NewMailAddress = "gtsecond@myges.fr",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Adresse mail déjà enregistrée");
    }

    [Fact(DisplayName =
        "Updating an inexisting account mail address\n" +
        "Should throw exception with code 404 and error type \"Compte introuvable\"")]
    public async void UpdateAccountMailAddressHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountMailAddressCommand(this.context).Handle(new UpdateMailDto()
            {
                MailAddress = "gtfirst@myges.fr",
                NewMailAddress = "gtsecond@myges.fr",
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Compte introuvable");
    }
}
