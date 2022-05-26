global using FluentAssertions;
global using Xunit;
global using Api.ExpositionModels;
global using Microsoft.AspNetCore.Mvc;
global using System.Collections.Generic;
global using System.Net;
global using System;
global using System.Threading.Tasks;
global using System.Linq;
using Api.Models;
using Api.Utilitaries;
using Microsoft.EntityFrameworkCore;
using Api.Controllers.AccountControllers;
using Api.Controllers.PostControllers;
using Api.Controllers.CommentaryControllers;
using Api.Controllers.GroupControllers;
using Api.Controllers.AuthenticationControllers;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class TestBase
{
    private readonly ModelsContext dbContext = new ModelsContext(new DbContextOptionsBuilder<ModelsContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    protected readonly Mapper mapper = new Mapper();
    private readonly DataValidator dataValidator = new DataValidator();
    private readonly Api.Utilitaries.StringComparer stringComparer = new Api.Utilitaries.StringComparer();
    private readonly IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    protected readonly AccountController accountsController;
    protected readonly PostController postController;
    protected readonly CommentaryController commentaryController;
    protected readonly GroupController groupController;
    protected readonly AuthenticationController authenticationController;

    protected TestBase()
    {
        this.accountsController = new AccountController(this.dbContext, this.mapper, this.dataValidator, this.stringComparer);
        this.postController = new PostController(this.dbContext, this.mapper);
        this.commentaryController = new CommentaryController(this.dbContext, this.mapper);
        this.groupController = new GroupController(this.dbContext, this.mapper, this.stringComparer);
        this.authenticationController = new AuthenticationController(this.dbContext, this.configuration);
    }
    
    protected void MockJwtAuthentication(AccountResource account, string role = "User")
    {
        PostController controller = this.postController;

        Mock<HttpContext> mock = new Mock<HttpContext>();
        mock.Setup(m => m.User).Returns(new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Email, account.MailAddress),
                    new Claim(ClaimTypes.Role, role),
                })));

        controller.ControllerContext.HttpContext = mock.Object;
    }

    /* useful for later testing
    protected async Task<ActionResult<string>> Authenticate(string mailAddress, string password)
    {
        return await this.authenticationController.Authenticate(new Authentication()
        {
            MailAddress = mailAddress,
            Password = password,
        });
    }

    private string GetJwt()
    {
        ActionResult<string> request = base.Authenticate(this.accountMail, this.accountPassword).Result;
        OkObjectResult result = request.Result as OkObjectResult;
        return result.Value.GetType().GetProperty("jwt").GetValue(result.Value) as string;
    }
    */
}
