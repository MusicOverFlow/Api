using Api.Handlers.Commands.AccountCommands;
using Api.Handlers.Queries.AccountQueries;

namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class FollowUnfollowAccountHandlerTests : TestBase
{
    [Fact(DisplayName =
        "Follow an existing, not followed account\n" +
        "Should add the account to followed accounts")]
    public async void FollowUnfollowAccountHandlerTest_1()
    {
        this.RegisterNewAccount("follower@myges.fr");
        this.RegisterNewAccount("followed@myges.fr");

        await this.handlers.Get<FollowUnfollowAccountCommand>().Handle(new FollowDto()
        {
            CallerMail = "follower@myges.fr",
            TargetMail = "followed@myges.fr",
        });

        Account follower = this.handlers.Get<ReadAccountSelfQuery>().Handle("follower@myges.fr").Result;
        Account followed = this.handlers.Get<ReadAccountByMailQuery>().Handle("followed@myges.fr").Result.First();
        follower.Follows.Should().ContainEquivalentOf(followed);
    }

    [Fact(DisplayName =
        "Follow an existing, already followed account\n" +
        "Should remove the account from followed accounts")]
    public async void FollowUnfollowAccountHandlerTest_2()
    {
        this.RegisterNewAccount("follower@myges.fr");
        this.RegisterNewAccount("followed@myges.fr");

        // Follow
        await this.handlers.Get<FollowUnfollowAccountCommand>().Handle(new FollowDto()
        {
            CallerMail = "follower@myges.fr",
            TargetMail = "followed@myges.fr",
        });

        // Unfollow
        await this.handlers.Get<FollowUnfollowAccountCommand>().Handle(new FollowDto()
        {
            CallerMail = "follower@myges.fr",
            TargetMail = "followed@myges.fr",
        });

        Account follower = this.handlers.Get<ReadAccountSelfQuery>().Handle("follower@myges.fr").Result;
        Account followed = this.handlers.Get<ReadAccountByMailQuery>().Handle("followed@myges.fr").Result.First();
        follower.Follows.Should().NotContainEquivalentOf(followed);
    }
}
