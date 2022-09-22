using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Api.Tests.EndToEndTests.AccountControllerTests;

public class CreateAccountTests : TestBase
{
    [Fact(DisplayName =
        "Account creation with valid request\n" +
        "Should return CreatedResult with status code 201")]
    public async void CreateAccountTest_1()
    {
        HttpRequest request = new HttpRequest();
        ActionResult response = await this.accountsController.Create("gtouchet@myges.fr", "123Pass!", null, null, null);
        response.Should().BeOfType<CreatedResult>();
    }
}
