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
}
