using Api.Handlers.Commands.AccountCommands;
using Api.Handlers.Commands.PostCommands;
using Api.Handlers.Queries.AccountQueries;
using Api.Handlers.Queries.PostQueries;

namespace Api.Tests.HandlersTests.PostHandlersTests;

public class LikeDislikePostHandlerTests : TestBase
{
    private Account account;
    private Post post;

    public LikeDislikePostHandlerTests()
    {
        this.account = this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
        {
            MailAddress = "gt@myges.fr",
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
            CallerMail = this.account.MailAddress,
            PostId = this.post.Id,
        });
        
        Post likedPost = this.handlers.Get<ReadPostByIdQuery>().Handle(this.post.Id).Result.First();
        
        likedPost.Likes.Should().ContainEquivalentOf(this.account);
    }
}
