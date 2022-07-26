namespace Api.Tests.PostControllerTests;

public class ReadHomePageTests : TestBase
{
    private readonly AccountResource account1, account2, account3;
    private PostResource postFromAccount1, postFromAccount2, postFromAccount3;

    public ReadHomePageTests()
    {
        this.account1 = this.CreateAccount("gtouchet1@myges.fr").Result;
        this.account2 = this.CreateAccount("gtouchet2@myges.fr").Result;
        this.account3 = this.CreateAccount("gtouchet3@myges.fr").Result;
    }

    private async Task<AccountResource> CreateAccount(string mail)
    {
        var request = await base.accountsController.Create(new CreateAccountRequest()
        {
            MailAddress = mail,
            Password = "123Pass!",
        });
        var result = request.Result as CreatedResult;

        return result.Value as AccountResource;
    }

    private async Task<PostResource> CreatePost(string title)
    {
        var request = await base.postController.Create(new CreatePost()
        {
            Title = title,
            Content = "content",
        }, groupId: null);
        var result = request.Result as CreatedResult;

        return result.Value as PostResource;
    }

    private async Task<CommentaryResource> CreateCommentary(Guid postId)
    {
        var request = await base.commentaryController.Create(new CreateCommentary()
        {
            Content = "content",
        }, postId);
        var result = request.Result as CreatedResult;

        return result.Value as CommentaryResource;
    }

    [Fact(DisplayName =
        "Home page of an account that posted a post and follow another account that posted another post\n" +
        "Should contain 2 posts / 3")]
    public async void HomePage_1()
    {
        base.MockJwtAuthentication(this.account1);
        this.postFromAccount1 = this.CreatePost("post 1").Result;
        await base.accountsController.FollowUnfollow(this.account2.MailAddress);

        base.MockJwtAuthentication(this.account2);
        this.postFromAccount2 = this.CreatePost("post 2").Result;


        base.MockJwtAuthentication(this.account3);
        this.postFromAccount3 = this.CreatePost("post 3").Result;

        base.MockJwtAuthentication(this.account1);

        var request = await base.postController.ReadHomePage();
        var result = request.Result as OkObjectResult;
        var posts = result.Value as List<PostResource>;

        List<string> postTitles = new List<string>();
        posts.ForEach(p => postTitles.Add(p.Title));

        posts.Count.Should().Be(2);
        postTitles.Should().Contain(this.postFromAccount1.Title);
        postTitles.Should().Contain(this.postFromAccount2.Title);
        postTitles.Should().NotContain(this.postFromAccount3.Title);
    }

    [Fact(DisplayName =
        "Home page of an account that posted a post and follow another account that posted another post and commented another poster account\n" +
        "Should contain all 3 posts")]
    public async void HomePage_2()
    {
        base.MockJwtAuthentication(this.account1);
        this.postFromAccount1 = this.CreatePost("post 1").Result;
        await base.accountsController.FollowUnfollow(this.account2.MailAddress);

        base.MockJwtAuthentication(this.account2);
        this.postFromAccount2 = this.CreatePost("post 2").Result;

        base.MockJwtAuthentication(this.account3);
        this.postFromAccount3 = this.CreatePost("post 3").Result;

        base.MockJwtAuthentication(this.account2);
        await this.CreateCommentary(this.postFromAccount3.Id);

        base.MockJwtAuthentication(this.account1);

        var request = await base.postController.ReadHomePage();
        var result = request.Result as OkObjectResult;
        var posts = result.Value as List<PostResource>;

        List<string> postTitles = new List<string>();
        posts.ForEach(p => postTitles.Add(p.Title));

        posts.Count.Should().Be(3);
        postTitles.Should().Contain(this.postFromAccount1.Title);
        postTitles.Should().Contain(this.postFromAccount2.Title);
        postTitles.Should().Contain(this.postFromAccount3.Title);
    }

    [Fact(DisplayName =
        "Home page of an account that posted a post\n" +
        "And posted a commentary on another account's post\n" +
        "And follow another account that posted another post and commented another poster account\n" +
        "Should contain all 3 posts")]
    public async void HomePage_3()
    {
        base.MockJwtAuthentication(this.account1);
        this.postFromAccount1 = this.CreatePost("post 1").Result;
        
        await base.accountsController.FollowUnfollow(this.account2.MailAddress);

        base.MockJwtAuthentication(this.account2);
        this.postFromAccount2 = this.CreatePost("post 2").Result;

        base.MockJwtAuthentication(this.account3);
        this.postFromAccount3 = this.CreatePost("post 3").Result;

        base.MockJwtAuthentication(this.account2);
        await this.CreateCommentary(this.postFromAccount3.Id);

        base.MockJwtAuthentication(this.account1);
        await this.CreateCommentary(this.postFromAccount3.Id);

        var request = await base.postController.ReadHomePage();
        var result = request.Result as OkObjectResult;
        var posts = result.Value as List<PostResource>;

        List<string> postTitles = new List<string>();
        posts.ForEach(p => postTitles.Add(p.Title));

        posts.Count.Should().Be(3);
        postTitles.Should().Contain(this.postFromAccount1.Title);
        postTitles.Should().Contain(this.postFromAccount2.Title);
        postTitles.Should().Contain(this.postFromAccount3.Title);
    }
}
