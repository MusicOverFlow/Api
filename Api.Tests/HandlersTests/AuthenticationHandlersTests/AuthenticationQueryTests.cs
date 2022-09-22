using Api.Handlers.Queries.AuthenticationQueries;

namespace Api.Tests.HandlersTests.AuthenticationHandlersTests;

public class AuthenticationQueryTests : TestBase
{
    private Account account;
    private AuthenticationQuery authenticationQuery;

    public AuthenticationQueryTests()
    {
        this.account = new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
        {
            MailAddress = "gt@myges.fr",
            Password = "myPass123?",
        }).Result;
        this.authenticationQuery = new AuthenticationQuery(this.context);
        this.authenticationQuery.Configuration = this.configuration;
    }

    [Fact(DisplayName = "Authenticating with valid credentials should create and return a JWT")]
    public async void AuthenticationQueryTest_1()
    {
        string jwt = await this.authenticationQuery.Handle(new AuthenticationDto()
        {
            MailAddress = this.account.MailAddress,
            Password = "myPass123?",
        });

        jwt.Should().NotBeNullOrEmpty();
    }

    [Fact(DisplayName = "Authenticating with wrong password should return an error code 401")]
    public async void AuthenticationQueryTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => this.authenticationQuery.Handle(new AuthenticationDto()
            {
                MailAddress = this.account.MailAddress,
                Password = "IForgotMyPass...",
            }));

        request.Content.StatusCode.Should().Be(401);
    }
}
