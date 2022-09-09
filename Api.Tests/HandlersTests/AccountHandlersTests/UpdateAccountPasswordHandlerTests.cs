using Api.Handlers.Commands.AccountCommands;
using Api.Handlers.Queries.AccountQueries;

namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountPasswordHandlerTests : TestBase
{
    [Fact(DisplayName =
        "Updating an account password with a valid password\n" +
        "Should update the account's password")]
    public async void UpdateAccountPasswordHandlerTest_1()
    {
        Account beforeUpdateAccount = await this.RegisterNewAccount("gt@myges.fr");

        await this.handlers.Get<UpdateAccountPasswordCommand>().Handle(new UpdatePasswordDto()
        {
            MailAddress = "gt@myges.fr",
            NewPassword = "456drowssaP?",
        });

        Account updatedAccount = await this.handlers.Get<ReadAccountSelfQuery>().Handle("gt@myges.fr");
        updatedAccount.PasswordHash.Should().NotEqual(beforeUpdateAccount.PasswordHash);
    }

    [Fact(DisplayName =
        "Updating an account password with an invalid password\n" +
        "Should throw exception with code 400 and error type \"Mot de passe invalide\"")]
    public async void UpdateAccountPasswordHandlerTest_2()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => this.handlers.Get<UpdateAccountPasswordCommand>().Handle(new UpdatePasswordDto()
            {
                MailAddress = "gt@myges.fr",
                NewPassword = "myPass",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Mot de passe invalide");
    }

    [Fact(DisplayName =
        "Updating an account password with an invalid password\n" +
        "Should not update the account's password")]
    public async void UpdateAccountPasswordHandlerTest_3()
    {
        Account beforeUpdateAccount = await this.RegisterNewAccount("gt@myges.fr");

        try
        {
            await this.handlers.Get<UpdateAccountPasswordCommand>().Handle(new UpdatePasswordDto()
            {
                MailAddress = "gt@myges.fr",
                NewPassword = "myPass",
            });
        }
        catch (HandlerException)
        {

        }

        Account updatedAccount = await this.handlers.Get<ReadAccountSelfQuery>().Handle("gt@myges.fr");
        updatedAccount.PasswordHash.Should().Equal(beforeUpdateAccount.PasswordHash);
    }

    [Fact(DisplayName =
        "Updating an inexisting account password\n" +
        "Should throw exception with code 404 and error type \"Compte introuvable\"")]
    public async void UpdateAccountPasswordHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => this.handlers.Get<UpdateAccountPasswordCommand>().Handle(new UpdatePasswordDto()
            {
                MailAddress = "gt@myges.fr",
                NewPassword = "123Password!",
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Compte introuvable");
    }
}
