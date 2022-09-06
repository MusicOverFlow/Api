using Api.Handlers.Commands.AccountCommands;
using Api.Handlers.Queries.AccountQueries;

namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountMailAddressHandlerTests : TestBase
{
    private async void RegisterNewAccount(string mailAddress)
    {
        await this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
        {
            MailAddress = mailAddress,
            Password = "123Password!",
        });
    }

    [Fact(DisplayName =
        "Updating an existing account mail address with a valid mail address\n" +
        "Should update the account mail address")]
    public async void UpdateAccountMailAddressHandlerTest_1()
    {
        this.RegisterNewAccount("gt@myges.fr");

        await this.handlers.Get<UpdateAccountMailAddressCommand>().Handle(new UpdateMailDto()
        {
            MailAddress = "gt@myges.fr",
            NewMailAddress = "gtnew@myges.fr",
        });

        Account updatedAccount = await this.handlers.Get<ReadAccountSelfQuery>().Handle("gtnew@myges.fr");
        updatedAccount.Should().NotBeNull();
    }

    [Fact(DisplayName =
        "Updating an existing account mail address with an invalid mail address\n" +
        "Should throw exception with code 404 and error type \"Adresse mail invalide\"")]
    public async void UpdateAccountMailAddressHandlerTest_2()
    {
        this.RegisterNewAccount("gt@myges.fr");
        
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => this.handlers.Get<UpdateAccountMailAddressCommand>().Handle(new UpdateMailDto()
            {
                MailAddress = "gt@myges.fr",
                NewMailAddress = "hello",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Adresse mail invalide");
    }

    [Fact(DisplayName =
        "Updating an existing account mail address with an already registered mail address\n" +
        "Should throw exception with code 404 and error type \"Adresse mail déjà enregistrée\"")]
    public async void UpdateAccountMailAddressHandlerTest_3()
    {
        this.RegisterNewAccount("gtfirst@myges.fr");
        this.RegisterNewAccount("gtsecond@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => this.handlers.Get<UpdateAccountMailAddressCommand>().Handle(new UpdateMailDto()
            {
                MailAddress = "gtfirst@myges.fr",
                NewMailAddress = "gtsecond@myges.fr",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Adresse mail déjà enregistrée");
    }
}
