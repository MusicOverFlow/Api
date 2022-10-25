namespace Api.Tests.HandlersTests.PostHandlersTests;

public class CreatePostCommandTests : TestBase
{
    private Account account;

    public CreatePostCommandTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
    }
    
    [Fact(DisplayName =
        "Creating a post with content and existing creator mail address\n" +
        "Should create the post")]
    public async void CreatePostHandlerTest_1()
    {
        Post post = await new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Content = "Post content",
        });

        post.Should().NotBeNull();
    }

    [Fact(DisplayName =
        "Creating a post with an inexisting creator mail address\n" +
        "Should throw exception code 404")]
    public async void CreatePostHandlerTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
            {
                CreatorMailAddress = "unknown@myges.fr",
                Content = "Post content",
            }));

        request.Content.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName =
        "Creating a post without content\n" +
        "Should throw exception with code 400 and error type \"Contenu du post vide\"")]
    public async void CreatePostHandlerTest_3()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
            {
                CreatorMailAddress = this.account.MailAddress,
                Content = "",
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Creating a post with a script and script language\n" +
        "Should create an URL to host the script")]
    public async void CreatePostHandlerTest_4()
    {
        Post post = await new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Content = "Post content",
            ScriptLanguage = Language.C.ToString(),
            Script = "printf(\"Hello, testing world\");",
        });

        post.ScriptLanguage.Should().Be(Language.C.ToString());
        post.Script.Should().Be($"https://post-scripts.s3.eu-west-3.amazonaws.com/{post.Id}");

        await this.container.DeletePostScript(post.Id);
    }

    [Fact(DisplayName =
        "Creating a post with a script but without a script language\n" +
        "Should discard the script of the post")]
    public async void CreatePostHandlerTest_5()
    {
        Post post = await new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Content = "Post content",
            Script = "printf(\"Hello, testing world\");",
        });

        post.Script.Should().Be(null);
    }

    [Fact(DisplayName =
        "Creating a post with a group ID\n" +
        "Should create the post in the group")]
    public async void CreatePostHandlerTest_6()
    {
        Group group = await this.RegisterNewGroup(this.account.MailAddress);

        Post post = await new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Content = "Post content",
            GroupId = group.Id,
        });

        post.Group.Id.Should().Be(group.Id);
    }

    [Fact(DisplayName =
        "Creating a post\n" +
        "Should add the post to the account's owned posts")]
    public async void CreatePostHandlerTest_7()
    {
        Post post = await new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Content = "Post content",
        });
        
        this.account.OwnedPosts.Should().Contain(post);
    }

    [Fact(DisplayName =
        "Creating a post with a inexisting group ID\n" +
        "Should throw exception code 404")]
    public async void CreatePostHandlerTest_8()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
            {
                CreatorMailAddress = this.account.MailAddress,
                Content = "Post content",
                GroupId = Guid.NewGuid(),
            }));

        request.Content.StatusCode.Should().Be(404);
    }
}
