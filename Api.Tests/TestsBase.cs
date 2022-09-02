global using FluentAssertions;
global using Xunit;
global using Microsoft.AspNetCore.Mvc;
global using System.Collections.Generic;
global using System.Net;
global using System;
global using System.Threading.Tasks;
global using System.Linq;
global using Api.Handlers.Utilitaries;
global using Api.Handlers.Kernel;
global using Api.Models.Entities;
global using Api.Handlers.Dtos;
using Api.Models;
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
using Api.Models.ExpositionModels.Resources;
using Api.Handlers;
using Api.Handlers.Commands.AccountCommands;

public class TestBase
{
    protected readonly HandlersContainer handlers;
    private readonly IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    protected readonly AccountController accountsController;
    protected readonly PostController postController;
    protected readonly CommentaryController commentaryController;
    protected readonly GroupController groupController;
    protected readonly AuthenticationController authenticationController;

    protected TestBase()
    {
        string databaseName = Guid.NewGuid().ToString();
        DbContextOptionsBuilder dbContextOptions = new DbContextOptionsBuilder()
            .UseLazyLoadingProxies()
            .UseInMemoryDatabase(databaseName);
        this.handlers = new HandlersContainer(() => new ModelsContext(dbContextOptions.Options));
        

        this.accountsController = new AccountController(this.handlers);
        this.postController = new PostController(this.handlers);
        this.commentaryController = new CommentaryController(this.handlers);
        this.groupController = new GroupController(this.handlers);
        this.authenticationController = new AuthenticationController(this.handlers, this.configuration);
    }

    protected async void RegisterNewAccount(string mailAddress)
    {
        await this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
        {
            MailAddress = mailAddress,
            Password = "123Password!",
        });
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
