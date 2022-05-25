using System.Linq;
using System.Threading.Tasks;

namespace Api.Tests.PostControllerTests;

public class CreateControllerTests : TestBase
{
    private readonly AccountResource account;
    
    public CreateControllerTests()
    {
        _ = base.CreateAccount("gtouchet@myges.fr", "123Pass!");
        this.account = this.GetAccount().Result;
    }
    
    private async Task<AccountResource> GetAccount()
    {
        var request = await base.ReadAccounts("gtouchet@myges.fr");
        var result = request.Result as OkObjectResult;
        var accounts = result.Value as List<AccountResource>;
        return accounts.First();
    }

    [Fact(DisplayName =
        "Post creation with valid title and content,\n" +
        "Should return CreatedResult with status code 201")]
    public async void PostCreation_1()
    {
        ActionResult<PostResource> request = await base.CreatePost(this.account, "Post about something I like", "Coffee.");

        request.Result.Should().BeOfType<CreatedResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact(DisplayName =
        "Post created by connected account,\n" +
        "Should be owned by this account")]
    public async void PostCreation_2()
    {
        ActionResult<PostResource> request = await base.CreatePost(this.account, "Post about the weather", "Das hot");
        CreatedResult result = request.Result as CreatedResult;
        PostResource post = result.Value as PostResource;

        post.Owner.MailAddress.Should().Be(this.account.MailAddress);
    }

    [Fact(DisplayName =
        "Post creation without title,\n" +
        "Should return BadRequestObjectResult with status code 400")]
    public async void PostCreation_3()
    {
        ActionResult<PostResource> request = await base.CreatePost(this.account, null, "Where's the title tho' ?");

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
    "Post creation without content,\n" +
    "Should return BadRequestObjectResult with status code 400")]
    public async void PostCreation_4()
    {
        ActionResult<PostResource> request = await base.CreatePost(this.account, "Content ? Hello ??", null);

        request.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName =
        "Post exposed resource should be of type PostResource,\n" +
        "And its owner account should be of type AccountResource")]
    public async void PostCreation_Mapping()
    {
        ActionResult<PostResource> request = await base.CreatePost(this.account, "Title", "Content");
        CreatedResult result = request.Result as CreatedResult;
        PostResource post = result.Value as PostResource;

        post.Should().BeOfType<PostResource>();
        post.Owner.Should().BeOfType<AccountResource_WithPosts>();
    }
}
