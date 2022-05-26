namespace Api.Tests.PostControllerTests;

public class CreateControllerTests : TestBase
{
    private readonly AccountResource account;
    private readonly GroupResource group;

    public CreateControllerTests()
    {
        this.account = this.CreateAccount().Result;
        base.MockJwtAuthentication(this.account);
        this.group = this.CreateGroup().Result;
    }
    
    private async Task<AccountResource> CreateAccount()
    {
        var request = await base.accountsController.Create(new CreateAccountRequest()
        {
            MailAddress = "gtouchet@myges.fr",
            Password = "123Pass!",
        });
        var result = request.Result as CreatedResult;
        
        return result.Value as AccountResource;
    }

    private async Task<GroupResource> CreateGroup()
    {
        var request = await base.groupController.Create(new CreateGroup()
        {
            Name = "My awesome group",
            Description = "This is an awesome group",
        });
        var result = request.Result as CreatedResult;

        return result.Value as GroupResource;
    }

    [Fact(DisplayName =
        "Post creation with valid title and content,\n" +
        "Should return CreatedResult with status code 201")]
    public async void PostCreation_1()
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Title = "Post about something I like",
            Content = "Coffee",
        }, groupId: null);

        request.Result.Should().BeOfType<CreatedResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact(DisplayName =
        "Post created by an account,\n" +
        "Should be owned by this account")]
    public async void PostCreation_2()
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Title = "Post about something I hate",
            Content = "Not sleeping enough",
        }, groupId: null);
        var result = request.Result as CreatedResult;
        var post = result.Value as PostResource;

        post.Owner.MailAddress.Should().Be(this.account.MailAddress);
    }

    [Fact(DisplayName =
        "Post creation without title,\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void PostCreation_3()
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Content = "Where's the title ?",
        }, groupId: null);

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Post creation without content,\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void PostCreation_4()
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Title = "Where's the content ?",
        }, groupId: null);

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Post creation with empty title,\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void PostCreation_5()
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Title = "",
            Content = "The title is empty",
        }, groupId: null);

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Post creation with empty content,\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void PostCreation_6()
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Title = "The content is empty",
            Content = "",
        }, groupId: null);

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Post creation by specifying an existing group ID,\n" +
        "Should link this post to the group")]
    public async void PostCreation_7()
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Title = "Am I in a group ?",
            Content = "Everyone's awesome here !",
        }, groupId: this.group.Id);
        var result = request.Result as CreatedResult;
        var post = result.Value as PostResource;

        post.Group.Id.Should().Be(this.group.Id);
    }

    [Fact(DisplayName =
        "Post creation by specifying a random ID,\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void PostCreation_8()
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Title = "Am I in a group ?",
            Content = "Seems not...",
        }, groupId: Guid.NewGuid());

        request.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact(DisplayName =
        "Exposed post after creation should be of type PostResource,\n" +
        "And its owner account should be of type AccountResource")]
    public async void PostCreation_Mapping()
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Title = "How's the weather ?",
            Content = "Das hot",
        }, groupId: null);
        var result = request.Result as CreatedResult;
        var post = result.Value as PostResource;

        post.Should().BeOfType<PostResource>();
        post.Owner.Should().BeOfType<AccountResource>();
    }
}
