using Microsoft.AspNetCore.Mvc;

namespace Api.Tests.IntegrationsTests.CodeControllerTests;

public class ExecuteCTests : TestBase
{
    [Fact(DisplayName = "Executing a printf should send the result to the client")]
    public async Task ExecuteCTests_1()
    {
        string script = "printf(\"Bonjour\");";
        
        this.MockHttpRequestWithStringBody(script);
        var result = await this.codeController.HandleC();
        
        result.As<OkObjectResult>().Value.Should().Be("Bonjour\n");
    }

    [Fact(DisplayName = "Executing a for loop should send the result to the client")]
    public async Task ExecuteCTests_2()
    {
        string script =
            @"for (int i = 0; i < 3; i += 1)
            {
                printf(""%d\n"", i);
            }";
        this.MockHttpRequestWithStringBody(script);
        var result = await this.codeController.HandleC();
        
        result.As<OkObjectResult>().Value.Should().Be("0\n1\n2\n\n");
    }

    [Fact(DisplayName = "Executing an infinite loop should send an error to the client")]
    public async Task ExecuteCTests_3()
    {
        string script =
            @"while (1)
            {
                printf(""kill me."");
            }";
        this.MockHttpRequestWithStringBody(script);
        var result = await this.codeController.HandleC();

        result.As<OkObjectResult>().Value.Should().Be("Error: infinite loop detected\n");
    }

    [Fact(DisplayName = "Executing a script with a missing ';' should display the error")]
    public async Task ExecuteCTests_4()
    {
        string script = "printf(\"Bonjour\")"; // missing ';'

        this.MockHttpRequestWithStringBody(script);
        var result = await this.codeController.HandleC();
        
        result.As<OkObjectResult>().Value.Should().NotBe("Bonjour\n");
    }
}
