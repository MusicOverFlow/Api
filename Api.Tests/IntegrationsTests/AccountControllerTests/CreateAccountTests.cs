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
        
    }
}
