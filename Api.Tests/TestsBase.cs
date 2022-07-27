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
using System.IO;

public class TestBase
{
    // TODO: try to use lazy loading context in tests
    private readonly ModelsContext dbContext = new ModelsContext(
        new DbContextOptionsBuilder<ModelsContext>()
            .UseLazyLoadingProxies()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
    protected readonly Mapper mapper = new Mapper();
    private readonly DataValidator dataValidator = new DataValidator();
    private readonly LevenshteinDistance stringComparer = new LevenshteinDistance();
    private readonly IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    private readonly ExceptionHandler exceptionHandler = new ExceptionHandler(new DirectoryInfo(Directory.GetCurrentDirectory()) + "/exceptions.json");

    protected readonly AccountController accountsController;
    protected readonly PostController postController;
    protected readonly CommentaryController commentaryController;
    protected readonly GroupController groupController;
    protected readonly AuthenticationController authenticationController;

    protected TestBase()
    {
        this.accountsController = new AccountController(this.dbContext, this.mapper, this.dataValidator, this.configuration, this.stringComparer, this.exceptionHandler);
        this.postController = new PostController(this.dbContext, this.mapper, this.exceptionHandler);
        this.commentaryController = new CommentaryController(this.dbContext, this.mapper, this.exceptionHandler);
        this.groupController = new GroupController(this.dbContext, this.mapper, this.configuration, this.stringComparer, this.exceptionHandler);
        this.authenticationController = new AuthenticationController(this.dbContext, this.configuration, this.exceptionHandler);
    }
    
    protected void MockJwtAuthentication(AccountResource account)
    {
        Mock<HttpContext> mock = new Mock<HttpContext>();
        
        mock.Setup(m => m.User).Returns(new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Email, account.MailAddress),
                })));

        this.accountsController.ControllerContext.HttpContext = mock.Object;
        this.postController.ControllerContext.HttpContext = mock.Object;
        this.groupController.ControllerContext.HttpContext = mock.Object;
        this.commentaryController.ControllerContext.HttpContext = mock.Object;
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
