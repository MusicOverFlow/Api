namespace Api.Tests.HandlersTests.PostHandlersTests;

public class LikeDislikePostCommandTests : TestBase
{
    private Account account;
    private Post post;

    public LikeDislikePostCommandTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
        this.post = this.RegisterNewPost(this.account.MailAddress).Result;
    }

    [Fact(DisplayName =
        "Liking an existing post\n" +
        "Should add the caller account to the post's likes")]
    public async void LikeDislikePostHandlerTest_1()
    {
        await new LikeDislikePostCommand(this.context).Handle(new LikeDislikeDto()
        {
            CallerMail = this.account.MailAddress,
            PostId = this.post.Id,
        });
        
        this.post.Likes.Count.Should().Be(1);
        this.post.Likes.Should().Contain(this.account);
    }

    [Fact(DisplayName =
        "Liking an existing, already liked post\n" +
        "Should remove the caller account from the post's likes")]
    public async void LikeDislikePostHandlerTest_2()
    {
        // Like
        await new LikeDislikePostCommand(this.context).Handle(new LikeDislikeDto()
        {
            CallerMail = this.account.MailAddress,
            PostId = this.post.Id,
        });
        
        // Dislike
        await new LikeDislikePostCommand(this.context).Handle(new LikeDislikeDto()
        {
            CallerMail = this.account.MailAddress,
            PostId = this.post.Id,
        });
        
        this.post.Likes.Count.Should().Be(0);
        this.post.Likes.Should().NotContain(this.account);
    }

    [Fact(DisplayName =
        "Liking a non existing post\n" +
        "Should throw exception code 404")]
    public async void LikeDislikePostHandlerTest_3()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new LikeDislikePostCommand(this.context).Handle(new LikeDislikeDto()
            {
                CallerMail = this.account.MailAddress,
                PostId = Guid.NewGuid(),
            }));

        request.Content.StatusCode.Should().Be(404);
    }
}
