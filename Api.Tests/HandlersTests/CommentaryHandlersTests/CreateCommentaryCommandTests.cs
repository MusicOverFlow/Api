﻿namespace Api.Tests.HandlersTests.CommentaryHandlersTests;

public class CreateCommentaryCommandTests : TestBase
{
    [Fact(DisplayName =
        "Creating a commentary with content and existing post's ID and creator mail\n" +
        "Should create the commentary and link it to the post")]
    public async void CreateCommentaryHandlerTest_1()
    {
        await this.RegisterNewAccount("gt@myges.fr");
        Post post = await this.RegisterNewPost("gt@myges.fr");

        Post postWithCommentary = await new CreateCommentaryCommand(this.context).Handle(new CreateCommentaryDto()
        {
            CreatorMailAddress = "gt@myges.fr",
            Content = "Commentary content",
            PostId = post.Id,
        });

        postWithCommentary.Commentaries.Count().Should().Be(1);
    }

    [Fact(DisplayName =
        "Creating a commentary with content and existing post's ID and creator mail\n" +
        "Should set the commentary's owner as its creator")]
    public async void CreateCommentaryHandlerTest_2()
    {
        Account account = await this.RegisterNewAccount("gt@myges.fr");
        Post post = await this.RegisterNewPost("gt@myges.fr");

        Post postWithCommentary = await new CreateCommentaryCommand(this.context).Handle(new CreateCommentaryDto()
        {
            CreatorMailAddress = account.MailAddress,
            Content = "Commentary content",
            PostId = post.Id,
        });

        postWithCommentary.Commentaries.First().Owner.Should().Be(account);
    }

    [Fact(DisplayName =
        "Creating a commentary with a script and script language\n" +
        "Should create an URL to host the script")]
    public async void CreateCommentaryHandlerTest_3()
    {
        await this.RegisterNewAccount("gt@myges.fr");
        Post post = await this.RegisterNewPost("gt@myges.fr");

        Post postWithCommentary = await new CreateCommentaryCommand(this.context).Handle(new CreateCommentaryDto()
        {
            CreatorMailAddress = "gt@myges.fr",
            Content = "Commentary content",
            PostId = post.Id,
            ScriptLanguage = Language.Python.ToString(),
            Script = "print('Hello, testing world')",
        });

        Commentary commentary = postWithCommentary.Commentaries.First();
        commentary.ScriptLanguage.Should().Be(Language.Python.ToString());
        commentary.ScriptUrl.Should().Be($"https://post-scripts.s3.eu-west-3.amazonaws.com/{commentary.Id}");

        await Blob.DeletePostScript(commentary.Id);
    }

    [Fact(DisplayName =
        "Creating a commentary with a script but without a script language\n" +
        "Should discard the script of the commentary")]
    public async void CreateCommentaryHandlerTest_4()
    {
        await this.RegisterNewAccount("gt@myges.fr");
        Post post = await this.RegisterNewPost("gt@myges.fr");

        Post postWithCommentary = await new CreateCommentaryCommand(this.context).Handle(new CreateCommentaryDto()
        {
            CreatorMailAddress = "gt@myges.fr",
            Content = "Commentary content",
            PostId = post.Id,
            Script = "print('Hello, testing world')",
        });

        Commentary commentary = postWithCommentary.Commentaries.First();
        commentary.ScriptUrl.Should().Be(null);
    }

    [Fact(DisplayName =
        "Creating a commentary linked to an inexisting post\n" +
        "Should throw exception with code 404 and error type \"Post ou commentaire introuvable\"")]
    public async void CreateCommentaryHandlerTest_5()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateCommentaryCommand(this.context).Handle(new CreateCommentaryDto()
            {
                CreatorMailAddress = "gt@myges.fr",
                Content = "Commentary content",
                PostId = Guid.NewGuid(),
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Post ou commentaire introuvable");
    }

    [Fact(DisplayName =
        "Creating a commentary linked to an inexisting post\n" +
        "Should throw exception with code 404 and error type \"Compte introuvable\"")]
    public async void CreateCommentaryHandlerTest_6()
    {
        await this.RegisterNewAccount("gt@myges.fr");
        Post post = await this.RegisterNewPost("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateCommentaryCommand(this.context).Handle(new CreateCommentaryDto()
            {
                CreatorMailAddress = "newGuy@myges.fr",
                Content = "Commentary content",
                PostId = post.Id,
            }));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Compte introuvable");
    }

    [Fact(DisplayName =
        "Creating a commentary without content\n" +
        "Should throw exception with code 400 and error type \"Contenu du post vide\"")]
    public async void CreateCommentaryHandlerTest_7()
    {
        await this.RegisterNewAccount("gt@myges.fr");
        Post post = await this.RegisterNewPost("gt@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateCommentaryCommand(this.context).Handle(new CreateCommentaryDto()
            {
                CreatorMailAddress = "gt@myges.fr",
                Content = string.Empty,
                PostId = post.Id,
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Contenu du post vide");
    }
}
