using System.Linq;
using System.Threading.Tasks;

namespace Api.Tests.PostControllerTests;

public class CreateControllerTests : TestBase
{
    private readonly AccountResource account;

    public CreateControllerTests()
    {
        _ = base.CreateAccount("gtouchet@myges.fr", "123Pass!");
        this.account = this.GetAccountResource().Result;
    }
    
    private async Task<AccountResource> GetAccountResource()
    {
        var request = await this.accountsController.Read("gtouchet@myges.fr");
        var result = request.Result as OkObjectResult;
        var accounts = result.Value as List<AccountResource>;
        return accounts.First();
    }

    [Fact(DisplayName =
        "Post creation with valid title and content,\n" +
        "Should return CreatedResult with status code 201")]
    public async void PostCreation_1()
    {
        ActionResult<PostResource> request = await base.CreatePost(this.account, "Post about something I like", "Coffee");

        request.Result.Should().BeOfType<CreatedResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }
}
