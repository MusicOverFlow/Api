using Api.Handlers.Containers;

namespace Api.Tests.HandlersTests.PostHandlersTests;

public class CreatePostCommandTests : TestBase
{
    [Fact(DisplayName =
        "Creating a post with content and existing creator mail address\n" +
        "Should create the post")]
    public async void CreatePostHandlerTest_1()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        Post post = await new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
        {
            CreatorMailAddress = "gt@myges.fr",
            Content = "Post content",
        });

        post.Should().NotBeNull();
    }

    [Fact(DisplayName =
        "Creating a post with an inexisting creator mail address\n" +
        "Should throw exception with code 404 and error type \"Compte introuvable\"")]
    public async void CreatePostHandlerTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
            {
                CreatorMailAddress = "gt@myges.fr",
                Content = "Post content",
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Compte introuvable");
    }

    [Fact(DisplayName =
        "Creating a post without content\n" +
        "Should throw exception with code 400 and error type \"Contenu du post vide\"")]
    public async void CreatePostHandlerTest_3()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
            {
                CreatorMailAddress = "gt@myges.fr",
                Content = "",
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Contenu du post vide");
    }

    [Fact(DisplayName =
        "Creating a post with a script and script language\n" +
        "Should create an URL to host the script")]
    public async void CreatePostHandlerTest_4()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        Post post = await new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
        {
            CreatorMailAddress = "gt@myges.fr",
            Content = "Post content",
            ScriptLanguage = Language.C.ToString(),
            Script = "printf(\"Hello, testing world\");",
        });

        post.ScriptLanguage.Should().Be(Language.C.ToString());
        post.ScriptUrl.Should().Be($"https://post-scripts.s3.eu-west-3.amazonaws.com/{post.Id}");

        await this.container.DeletePostScript(post.Id);
    }

    [Fact(DisplayName =
        "Creating a post with a script but without a script language\n" +
        "Should discard the script of the post")]
    public async void CreatePostHandlerTest_5()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        Post post = await new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
        {
            CreatorMailAddress = "gt@myges.fr",
            Content = "Post content",
            Script = "printf(\"Hello, testing world\");",
        });

        post.ScriptUrl.Should().Be(null);
    }

    [Fact(DisplayName =
        "Creating a post with a group ID\n" +
        "Should create the post in the group")]
    public async void CreatePostHandlerTest_6()
    {
        await this.RegisterNewAccount("gt@myges.fr");
        Group group = await this.RegisterNewGroup("gt@myges.fr");

        Post post = await new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
        {
            CreatorMailAddress = "gt@myges.fr",
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
        await this.RegisterNewAccount("gt@myges.fr");

        Post post = await new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
        {
            CreatorMailAddress = "gt@myges.fr",
            Content = "Post content",
        });

        Account accountWithPost = this.context.Accounts
            .FirstOrDefault(a => a.MailAddress.Equals("gt@myges.fr"));

        accountWithPost.OwnedPosts.Should().Contain(post);
    }

    [Fact(DisplayName =
        "Creating a post with a inexisting group ID\n" +
        "Should throw exception with code 404 and error type \"Groupe introuvable\"")]
    public async void CreatePostHandlerTest_8()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreatePostCommand(this.context, this.container).Handle(new CreatePostDto()
            {
                CreatorMailAddress = "gt@myges.fr",
                Content = "Post content",
                GroupId = Guid.NewGuid(),
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Groupe introuvable");
    }
}
