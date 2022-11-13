namespace Api.Tests.HandlersTests.CommentaryHandlersTests;

public class CreateCommentaryCommandTests : TestBase
{
    private Account account;
    private Post post;

    public CreateCommentaryCommandTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
        this.post = this.RegisterNewPost(this.account.MailAddress).Result;
    }

    [Fact(DisplayName =
        "Creating a commentary with content and existing post's ID and creator mail\n" +
        "Should create the commentary and link it to the post")]
    public async void CreateCommentaryHandlerTest_1()
    {
        Post postWithCommentary = await new CreateCommentaryCommand(this.context, this.container).Handle(new CreateCommentaryDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Content = "Commentary content",
            PostId = this.post.Id,
        });

        postWithCommentary.Commentaries.Should().HaveCount(1);
    }

    [Fact(DisplayName =
        "Creating a commentary with content and existing post's ID and creator mail\n" +
        "Should set the commentary's owner as its creator")]
    public async void CreateCommentaryHandlerTest_2()
    {
        Post postWithCommentary = await new CreateCommentaryCommand(this.context, this.container).Handle(new CreateCommentaryDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Content = "Commentary content",
            PostId = this.post.Id,
        });

        postWithCommentary.Commentaries.First().Owner.Should().Be(this.account);
    }

    [Fact(DisplayName =
        "Creating a commentary with a script and script language\n" +
        "Should create an URL to host the script")]
    public async void CreateCommentaryHandlerTest_3()
    {
        Post postWithCommentary = await new CreateCommentaryCommand(this.context, this.container).Handle(new CreateCommentaryDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Content = "Commentary content",
            PostId = this.post.Id,
            ScriptLanguage = Language.Python.ToString(),
            Script = "print('Hello, testing world')",
        });

        Commentary commentary = postWithCommentary.Commentaries.First();
        commentary.ScriptLanguage.Should().Be(Language.Python.ToString());
        commentary.Script.Should().Be($"https://post-scripts.s3.eu-west-3.amazonaws.com/{commentary.Id}");

        await this.container.DeletePostScript(commentary.Id);
    }

    [Fact(DisplayName =
        "Creating a commentary with a script but without a script language\n" +
        "Should discard the script of the commentary")]
    public async void CreateCommentaryHandlerTest_4()
    {
        Post postWithCommentary = await new CreateCommentaryCommand(this.context, this.container).Handle(new CreateCommentaryDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Content = "Commentary content",
            PostId = this.post.Id,
            Script = "print('Hello, testing world')",
        });

        postWithCommentary.Commentaries.First().Script.Should().Be(null);
    }

    [Fact(DisplayName =
        "Creating a commentary linked to an inexisting post\n" +
        "Should throw exception code 404")]
    public async void CreateCommentaryHandlerTest_5()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateCommentaryCommand(this.context, this.container).Handle(new CreateCommentaryDto()
            {
                CreatorMailAddress = this.account.MailAddress,
                Content = "Commentary content",
                PostId = Guid.NewGuid(),
            }));

        request.Content.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName =
        "Creating a commentary linked to an inexisting post\n" +
        "Should throw exception code 404")]
    public async void CreateCommentaryHandlerTest_6()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateCommentaryCommand(this.context, this.container).Handle(new CreateCommentaryDto()
            {
                CreatorMailAddress = "unknown@myges.fr",
                Content = "Commentary content",
                PostId = this.post.Id,
            }));

        request.Content.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName =
        "Creating a commentary without content\n" +
        "Should throw exception code 400")]
    public async void CreateCommentaryHandlerTest_7()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateCommentaryCommand(this.context, this.container).Handle(new CreateCommentaryDto()
            {
                CreatorMailAddress = this.account.MailAddress,
                Content = string.Empty,
                PostId = this.post.Id,
            }));

        request.Content.StatusCode.Should().Be(400);
    }
}
