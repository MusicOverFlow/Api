namespace Api.Tests.PostControllerTests;

public class ReadControllerTests : TestBase
{
    private readonly AccountResource account;
    private readonly PostResource post;

    public ReadControllerTests()
    {
        this.account = this.CreateAccount().Result;
        base.MockJwtAuthentication(this.account);
        this.post = this.CreatePost().Result;
    }

    private async Task<AccountResource> CreateAccount()
    {
        var request = await base.accountsController.Create(
                mailAddress: "gtouchet@myges.fr",
                password: "123Pass!",
                firstname: "Guillaume",
                lastname: "Touchet",
                pseudonym: null,
                profilPic: null);
        var result = request.Result as CreatedResult;

        return result.Value as AccountResource;
    }

    private async Task<PostResource> CreatePost()
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Title = "1st post",
            Content = "1st post content",
        }, groupId: null);
        var result = request.Result as CreatedResult;

        return result.Value as PostResource;
    }

    [Fact(DisplayName =
        "Read posts without specifying an ID\n" +
        "Should return OkObjectResult with status code 200")]
    public async void PostReading_1()
    {
        var request = await base.postController.Read();

        request.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact(DisplayName =
        "Read posts without specifying an ID\n" +
        "Should return all posts")]
    public async void PostReading_2()
    {
        await base.postController.Create(new CreatePost()
        {
            Title = "2nd post",
            Content = "2nd post content",
        }, groupId: null);

        var request = await base.postController.Read();
        var result = request.Result as OkObjectResult;
        var posts = result.Value as List<PostResource>;

        posts.Count.Should().Be(2);
    }

    [Fact(DisplayName =
        "Read posts specifying an ID\n" +
        "Should return OkObjectResult with status code 200")]
    public async void PostReading_3()
    {
        var request = await base.postController.Read(this.post.Id);

        request.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact(DisplayName =
        "Read posts specifying an ID\n" +
        "Should return the post resource")]
    public async void PostReading_4()
    {
        var request = await base.postController.Read(this.post.Id);
        var result = request.Result as OkObjectResult;
        var posts = result.Value as List<PostResource>;

        posts.First().Id.Should().Be(this.post.Id);
    }

    [Fact(DisplayName =
        "Read posts specifying a random GUID\n" +
        "Should return NotFoundObjectResult with status code 404")]
    public async void PostReading_5()
    {
        var request = await base.postController.Read(Guid.NewGuid());

        request.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact(DisplayName =
        "Post reading results should be exposed as PostResource_WithCommentaries_AndLikes")]
    public async void PostCreation_DataMapping()
    {
        var request = await base.postController.Read();
        var result = request.Result as OkObjectResult;

        result.Value.Should().BeOfType<List<PostResource>>();
    }
}
