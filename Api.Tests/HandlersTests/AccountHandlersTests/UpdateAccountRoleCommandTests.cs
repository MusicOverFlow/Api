namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountRoleCommandTests : TestBase
{
    /*
     * Note : il faut être admin pour pouvoir modifier le rôle d'un compte
     * Cette vérification est faite par le middleware de EFCore, et n'est
     * donc pas testée par le Handler
     */

    [Fact(DisplayName =
        "Creating an account\n" +
        "Should set the role to User")]
    public async void UpdateAccountRoleHandlerTest_1()
    {
        Account account = await new CreateAccountCommand(this.context).Handle(new CreateAccountDto()
        {
            MailAddress = "gt@myges.fr",
            Password = "123Password!",
        });
        
        account.Role.Should().Be(Role.User.ToString());
    }

    [Fact(DisplayName =
        "Updating an account role with a valid role\n" +
        "Should update the account's role")]
    public async void UpdateAccountRoleHandlerTest_2()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        Account account = await new UpdateAccountRoleCommand(this.context).Handle(new UpdateAccountRoleDto()
        {
            MailAddress = "gt@myges.fr",
            Role = Role.Moderator.ToString(),
        });

        account.Role.Should().Be(Role.Moderator.ToString());
    }

    [Fact(DisplayName =
        "Updating an account role with an invalid role\n" +
        "Should throw exception with code 400 and error type \"Rôle invalide\"")]
    public async void UpdateAccountRoleHandlerTest_3()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountRoleCommand(this.context).Handle(new UpdateAccountRoleDto()
            {
                MailAddress = "gt@myges.fr",
                Role = "Aviateur",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Rôle invalide");
    }

    [Fact(DisplayName =
        "Updating an inexisting account role\n" +
        "Should throw exception with code 404 and error type \"Compte introuvable\"")]
    public async void UpdateAccountRoleHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountRoleCommand(this.context).Handle(new UpdateAccountRoleDto()
            {
                MailAddress = "gt@myges.fr",
                Role = Role.Moderator.ToString(),
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Compte introuvable");
    }
}
