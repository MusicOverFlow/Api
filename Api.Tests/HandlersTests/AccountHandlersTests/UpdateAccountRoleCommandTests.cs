namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountRoleCommandTests : TestBase
{
    private Account account;

    public UpdateAccountRoleCommandTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
    }
    
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
        Account account = await new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
        {
            MailAddress = "newGuy@myges.fr",
            Password = "123Password!",
        });
        
        account.Role.Should().Be(Role.User.ToString());
    }

    [Fact(DisplayName =
        "Updating an account role with a valid role\n" +
        "Should update the account's role")]
    public async void UpdateAccountRoleHandlerTest_2()
    {
        Account account = await new UpdateAccountRoleCommand(this.context).Handle(new UpdateAccountRoleDto()
        {
            MailAddress = this.account.MailAddress,
            Role = Role.Moderator.ToString(),
        });

        account.Role.Should().Be(Role.Moderator.ToString());
    }

    [Fact(DisplayName =
        "Updating an account role with an invalid role\n" +
        "Should throw exception code 400")]
    public async void UpdateAccountRoleHandlerTest_3()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountRoleCommand(this.context).Handle(new UpdateAccountRoleDto()
            {
                MailAddress = this.account.MailAddress,
                Role = "Aviateur",
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Updating an inexisting account role\n" +
        "Should throw exception code 404")]
    public async void UpdateAccountRoleHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountRoleCommand(this.context).Handle(new UpdateAccountRoleDto()
            {
                MailAddress = "unknown@myges.fr",
                Role = Role.Moderator.ToString(),
            }));

        request.Content.StatusCode.Should().Be(404);
    }
}
