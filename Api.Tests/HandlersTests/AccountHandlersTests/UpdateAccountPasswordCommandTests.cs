using System.Security.Cryptography;

namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountPasswordCommandTests : TestBase
{
    private bool CompareTo(string password, byte[] hash, byte[] salt)
    {
        return new HMACSHA512(salt)
            .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
            .SequenceEqual(hash);
    }

    [Fact(DisplayName =
        "Updating an account password with a valid password\n" +
        "Should update the account's password")]
    public async void UpdateAccountPasswordHandlerTest_1()
    {
        Account account = await this.RegisterNewAccount("gt@myges.fr");
        byte[] beforeUpdateHash = account.PasswordHash;
        byte[] beforeUpdateSalt = account.PasswordSalt;

        Account updatedAccount = await new UpdateAccountPasswordCommand(this.context).Handle(new UpdatePasswordDto()
        {
            MailAddress = account.MailAddress,
            NewPassword = "456drowssaP?",
        });

        updatedAccount.PasswordHash.Should().NotBeEquivalentTo(beforeUpdateHash);
    }

    [Fact(DisplayName =
        "Updating an account password with an invalid password\n" +
        "Should throw exception with code 400 and error type \"Mot de passe invalide\"")]
    public async void UpdateAccountPasswordHandlerTest_2()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountPasswordCommand(this.context).Handle(new UpdatePasswordDto()
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
        Account updatedAccount = beforeUpdateAccount;
        
        try
        {
            updatedAccount = await new UpdateAccountPasswordCommand(this.context).Handle(new UpdatePasswordDto()
            {
                MailAddress = "gt@myges.fr",
                NewPassword = "myPass",
            });
        }
        catch (HandlerException)
        {

        }
        
        updatedAccount.PasswordHash.Should().Equal(beforeUpdateAccount.PasswordHash);
    }

    [Fact(DisplayName =
        "Updating an inexisting account password\n" +
        "Should throw exception with code 404 and error type \"Compte introuvable\"")]
    public async void UpdateAccountPasswordHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountPasswordCommand(this.context).Handle(new UpdatePasswordDto()
            {
                MailAddress = "gt@myges.fr",
                NewPassword = "123Password!",
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Compte introuvable");
    }
}
