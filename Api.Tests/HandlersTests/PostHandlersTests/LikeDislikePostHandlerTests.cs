using Api.Handlers.Commands.AccountCommands;
using Api.Handlers.Commands.PostCommands;
using Api.Handlers.Queries.PostQueries;

namespace Api.Tests.HandlersTests.PostHandlersTests;

public class LikeDislikePostHandlerTests : TestBase
{
    private const string ACCOUNT_MAIL = "gt@myges.fr";
    private Account account;
    private Post post;

    public LikeDislikePostHandlerTests()
    {
        this.account = this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
        {
            MailAddress = ACCOUNT_MAIL,
            Password = "123Password!",
        }).Result;

        this.post = this.handlers.Get<CreatePostCommand>().Handle(new CreatePostDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Content = "Post content",
        }).Result;
    }

    [Fact(DisplayName =
        "Liking an existing post\n" +
        "Should add the caller account to the post's likes")]
    public async void LikeDislikePostHandlerTest_1()
    {
        await this.handlers.Get<LikeDislikePostCommand>().Handle(new LikeDislikeDto()
        {
            CallerMail = ACCOUNT_MAIL,
            PostId = this.post.Id,
        });

        Post likedPost = this.handlers.Get<ReadPostByIdQuery>().Handle(this.post.Id).Result.First();

        likedPost.Likes.Count.Should().Be(1);
        likedPost.Likes.First().MailAddress.Should().Be(ACCOUNT_MAIL);
    }

    [Fact(DisplayName =
        "Liking an existing, already liked post\n" +
        "Should remove the caller account from the post's likes")]
    public async void LikeDislikePostHandlerTest_2()
    {
        // Like
        await this.handlers.Get<LikeDislikePostCommand>().Handle(new LikeDislikeDto()
        {
            CallerMail = this.account.MailAddress,
            PostId = this.post.Id,
        });

        // Dislike
        await this.handlers.Get<LikeDislikePostCommand>().Handle(new LikeDislikeDto()
        {
            CallerMail = this.account.MailAddress,
            PostId = this.post.Id,
        });

        Post likedPost = this.handlers.Get<ReadPostByIdQuery>().Handle(this.post.Id).Result.First();

        likedPost.Likes.Count.Should().Be(0);
    }

    [Fact(DisplayName =
        "Liking a non existing post\n" +
        "Should throw exception with code 404 and error type \"Post ou commentaire introuvable\"")]
    public async void LikeDislikePostHandlerTest_3()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => this.handlers.Get<LikeDislikePostCommand>().Handle(new LikeDislikeDto()
            {
                CallerMail = this.account.MailAddress,
                PostId = Guid.NewGuid(),
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Post ou commentaire introuvable");
    }
}
