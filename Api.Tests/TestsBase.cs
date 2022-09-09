global using FluentAssertions;
global using Xunit;
global using System.Collections.Generic;
global using System;
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
using Api.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Api.Handlers.Commands.AccountCommands;
using System.Threading.Tasks;
using Api.Handlers.Commands.PostCommands;
using Api.Handlers.Commands.GroupCommands;

public class TestBase
{
    private readonly IServiceCollection services;
    protected readonly HandlersContainer handlers;
    private readonly IConfiguration configuration;

    protected readonly AccountController accountsController;
    protected readonly PostController postController;
    protected readonly CommentaryController commentaryController;
    protected readonly GroupController groupController;
    protected readonly AuthenticationController authenticationController;
    
    protected readonly string fakeAccountForAwsTesting = "ffe2eff4-c1e2-44e5-9b7e-d1c8a014d295_fake_account_for_testing@test.fr";
    protected readonly string fakeGroupForAwsTesting = "12d77557-b1af-4c5d-b912-f8e684f1068d_fake_group_for_testing";

    protected TestBase()
    {
        this.services = new ServiceCollection()
            .AddDbContext<ModelsContext>(
                options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()),
                contextLifetime: ServiceLifetime.Transient);
        this.handlers = new HandlersContainer(this.services.BuildServiceProvider());
        this.configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        this.accountsController = new AccountController(this.handlers);
        this.postController = new PostController(this.handlers);
        this.commentaryController = new CommentaryController(this.handlers);
        this.groupController = new GroupController(this.handlers);
        this.authenticationController = new AuthenticationController(this.handlers, this.configuration);
    }

    protected async Task<Account> RegisterNewAccount(string mailAddress)
    {
        return await this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
        {
            MailAddress = mailAddress,
            Password = "123Password!",
        });
    }

    protected async Task<Post> RegisterNewPost(string creatorMailAddress)
    {
        return await this.handlers.Get<CreatePostCommand>().Handle(new CreatePostDto()
        {
            CreatorMailAddress = creatorMailAddress,
            Content = "Post content",
        });
    }

    protected async Task<Group> RegisterNewGroup(string creatorMailAddress)
    {
        return await this.handlers.Get<CreateGroupCommand>().Handle(new CreateGroupDto()
        {
            CreatorMailAddress = creatorMailAddress,
            Name = "Group name",
        });
    }

    protected void MockJwtAuthentication(Account account)
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
