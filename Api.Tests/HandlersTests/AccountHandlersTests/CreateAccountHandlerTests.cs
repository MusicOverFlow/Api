using Api.Handlers.Commands.AccountCommands;
using Microsoft.AspNetCore.Http;

namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class CreateAccountHandlerTests : TestBase
{
    [Fact(DisplayName =
        "Account creation with valid request\n" +
        "Should create the account")]
    public async void CreateAccountHandlerTest_1()
    {
        Account account = await this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
        {
            MailAddress = "gt@myges.fr",
            Password = "123Password!",
        });

        account.Should().NotBeNull();
    }

    [Fact(DisplayName =
        "Account creation with any invalid field\n" +
        "Should not create the account")]
    public async void CreateAccountHandlerTest_2()
    {
        Account account = null;
        try
        {
            account = await this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
            {
                MailAddress = null,
                Password = "123Password!",
            });
        }
        catch (HandlerException)
        {

        }

        account.Should().BeNull();
    }

    [Fact(DisplayName =
        "Account creation with invalid mail address\n" +
        "Should throw exception with code 400 and error type \"Adresse mail invalide\"")]
    public async void CreateAccountHandlerTest_3()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
            {
                MailAddress = "invalidMailAddress",
                Password = "123Password!",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Adresse mail invalide");
    }

    [Fact(DisplayName =
        "Account creation with invalid password\n" +
        "Should throw exception with code 400 and error type \"Mot de passe invalide\"")]
    public async void CreateAccountHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
            {
                MailAddress = "gt@myges.fr",
                Password = "123",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Mot de passe invalide");
    }

    [Fact(DisplayName =
        "Account creation with already registered mail address\n" +
        "Should throw exception with code 400 and error type \"Adresse mail déjà enregistrée\"")]
    public async void CreateAccountHandlerTest_5()
    {
        await this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
        {
            MailAddress = "gt@myges.fr",
            Password = "123Password!",
        });

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
            {
                MailAddress = "gt@myges.fr",
                Password = "123Password!",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Adresse mail déjà enregistrée");
    }

    [Fact(DisplayName =
        "Account creation with an invalid profil pic file format\n" +
        "Should throw exception with code 400 and error type \"Format de fichier invalide\"")]
    public async void CreateAccountHandlerTest_6()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
            {
                MailAddress = "gt@myges.fr",
                Password = "123Password!",
                ProfilPic = new FormFile(
                    baseStream: null,
                    baseStreamOffset: 0,
                    length: 1,
                    name: "",
                    fileName: "myProfilPic.mp3"),
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Format de fichier invalide");
    }
}
