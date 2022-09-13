namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class FollowUnfollowAccountCommandTests : TestBase
{
    [Fact(DisplayName =
        "Follow an existing, not followed account\n" +
        "Should add the account to followed accounts")]
    public async void FollowUnfollowAccountHandlerTest_1()
    {
        await this.RegisterNewAccount("follower@myges.fr");
        await this.RegisterNewAccount("followed@myges.fr");

        await new FollowUnfollowAccountCommand(this.context).Handle(new FollowDto()
        {
            CallerMail = "follower@myges.fr",
            TargetMail = "followed@myges.fr",
        });

        Account follower = await new ReadAccountSelfQuery(this.context).Handle("follower@myges.fr");
        Account followed = await new ReadAccountSelfQuery(this.context).Handle("followed@myges.fr");
        follower.Follows.First().MailAddress.Should().Be(followed.MailAddress);
    }

    [Fact(DisplayName =
        "Follow an existing, already followed account\n" +
        "Should remove the account from followed accounts")]
    public async void FollowUnfollowAccountHandlerTest_2()
    {
        await this.RegisterNewAccount("follower@myges.fr");
        await this.RegisterNewAccount("followed@myges.fr");

        // Follow
        await new FollowUnfollowAccountCommand(this.context).Handle(new FollowDto()
        {
            CallerMail = "follower@myges.fr",
            TargetMail = "followed@myges.fr",
        });

        // Unfollow
        await new FollowUnfollowAccountCommand(this.context).Handle(new FollowDto()
        {
            CallerMail = "follower@myges.fr",
            TargetMail = "followed@myges.fr",
        });

        Account follower = await new ReadAccountSelfQuery(this.context).Handle("follower@myges.fr");
        Account followed = await new ReadAccountSelfQuery(this.context).Handle("followed@myges.fr");
        follower.Follows.Should().NotContainEquivalentOf(followed);
    }

    [Fact(DisplayName =
        "Following yourself\n" +
        "Should throw exception with code 400 and type \"Tentative de self-follow\"")]
    public async void FollowUnfollowAccountHandlerTest_3()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new FollowUnfollowAccountCommand(this.context).Handle(new FollowDto()
            {
                CallerMail = "gt@myges.fr",
                TargetMail = "gt@myges.fr",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Tentative de self-follow");
    }

    [Fact(DisplayName =
        "Following an inexisting account\n" +
        "Should throw exception with code 404 and type \"Compte introuvable\"")]
    public async void FollowUnfollowAccountHandlerTest_4()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new FollowUnfollowAccountCommand(this.context).Handle(new FollowDto()
            {
                CallerMail = "gt@myges.fr",
                TargetMail = "gt2@myges.fr",
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Compte introuvable");
    }
}
