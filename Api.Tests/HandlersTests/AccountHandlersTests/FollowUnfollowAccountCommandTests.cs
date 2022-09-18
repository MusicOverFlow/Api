namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class FollowUnfollowAccountCommandTests : TestBase
{
    private Account follower, followed;
    
    public FollowUnfollowAccountCommandTests()
    {
        this.follower = this.RegisterNewAccount("follower@myges.fr").Result;
        this.followed = this.RegisterNewAccount("followed@myges.fr").Result;
    }

    [Fact(DisplayName =
        "Follow an existing, not followed account\n" +
        "Should add the account to followed accounts")]
    public async void FollowUnfollowAccountHandlerTest_1()
    {
        await new FollowUnfollowAccountCommand(this.context).Handle(new FollowDto()
        {
            CallerMail = this.follower.MailAddress,
            TargetMail = this.followed.MailAddress,
        });
        
        follower.Follows.Should().Contain(followed);
    }

    [Fact(DisplayName =
        "Follow an existing, already followed account\n" +
        "Should remove the account from followed accounts")]
    public async void FollowUnfollowAccountHandlerTest_2()
    {
        // Follow
        await new FollowUnfollowAccountCommand(this.context).Handle(new FollowDto()
        {
            CallerMail = this.follower.MailAddress,
            TargetMail = this.followed.MailAddress,
        });

        // Unfollow
        await new FollowUnfollowAccountCommand(this.context).Handle(new FollowDto()
        {
            CallerMail = this.follower.MailAddress,
            TargetMail = this.followed.MailAddress,
        });
        
        follower.Follows.Should().NotContain(followed);
    }

    [Fact(DisplayName =
        "Following yourself\n" +
        "Should throw exception code 400")]
    public async void FollowUnfollowAccountHandlerTest_3()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new FollowUnfollowAccountCommand(this.context).Handle(new FollowDto()
            {
                CallerMail = this.follower.MailAddress,
                TargetMail = this.follower.MailAddress,
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Following an inexisting account\n" +
        "Should throw exception code 404")]
    public async void FollowUnfollowAccountHandlerTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new FollowUnfollowAccountCommand(this.context).Handle(new FollowDto()
            {
                CallerMail = this.follower.MailAddress,
                TargetMail = "nonExistingAccount@myges.fr",
            }));

        request.Content.StatusCode.Should().Be(404);
    }
}
