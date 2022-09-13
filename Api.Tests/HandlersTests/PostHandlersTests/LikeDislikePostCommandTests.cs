namespace Api.Tests.HandlersTests.PostHandlersTests;

public class LikeDislikePostCommandTests : TestBase
{
    [Fact(DisplayName =
        "Liking an existing post\n" +
        "Should add the caller account to the post's likes")]
    public async void LikeDislikePostHandlerTest_1()
    {
        Account account = await this.RegisterNewAccount("gt@myges.fr");
        Post post = await this.RegisterNewPost(account.MailAddress);
        
        await new LikeDislikePostCommand(this.context).Handle(new LikeDislikeDto()
        {
            CallerMail = account.MailAddress,
            PostId = post.Id,
        });

        Post likedPost = this.context.Posts
            .FirstOrDefault(p => p.Id.Equals(post.Id));
        
        likedPost.Likes.Count.Should().Be(1);
        likedPost.Likes.Should().Contain(account);
    }

    [Fact(DisplayName =
        "Liking an existing, already liked post\n" +
        "Should remove the caller account from the post's likes")]
    public async void LikeDislikePostHandlerTest_2()
    {
        Account account = await this.RegisterNewAccount("gt@myges.fr");
        Post post = await this.RegisterNewPost("gt@myges.fr");

        // Like
        await new LikeDislikePostCommand(this.context).Handle(new LikeDislikeDto()
        {
            CallerMail = "gt@myges.fr",
            PostId = post.Id,
        });

        // Dislike
        await new LikeDislikePostCommand(this.context).Handle(new LikeDislikeDto()
        {
            CallerMail = "gt@myges.fr",
            PostId = post.Id,
        });

        Post likedPost = this.context.Posts
            .FirstOrDefault(p => p.Id.Equals(post.Id));

        likedPost.Likes.Count.Should().Be(0);
    }

    [Fact(DisplayName =
        "Liking a non existing post\n" +
        "Should throw exception with code 404 and error type \"Post ou commentaire introuvable\"")]
    public async void LikeDislikePostHandlerTest_3()
    {
        Account account = await this.RegisterNewAccount("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new LikeDislikePostCommand(this.context).Handle(new LikeDislikeDto()
            {
                CallerMail = account.MailAddress,
                PostId = Guid.NewGuid(),
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Post ou commentaire introuvable");
    }
}
