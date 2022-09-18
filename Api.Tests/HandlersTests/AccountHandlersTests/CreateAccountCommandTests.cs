using Microsoft.AspNetCore.Http;

namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class CreateAccountCommandTests : TestBase
{
    [Fact(DisplayName =
        "Account creation with valid request\n" +
        "Should create the account")]
    public async void CreateAccountHandlerTest_1()
    {
        Account account = await new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
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
            account = await new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
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
        "Should throw exception code 400")]
    public async void CreateAccountHandlerTest_3()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
            {
                MailAddress = "invalidMailAddress",
                Password = "123Password!",
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Account creation with invalid password\n" +
        "Should throw exception code 400")]
    public async void CreateAccountHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
            {
                MailAddress = "gt@myges.fr",
                Password = "123",
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Account creation with already registered mail address\n" +
        "Should throw exception code 400")]
    public async void CreateAccountHandlerTest_5()
    {
        await new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
        {
            MailAddress = "gt@myges.fr",
            Password = "123Password!",
        });

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
            {
                MailAddress = "gt@myges.fr",
                Password = "123Password!",
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Account creation with an invalid profil pic file format\n" +
        "Should throw exception code 400")]
    public async void CreateAccountHandlerTest_6()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
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
    }
}
