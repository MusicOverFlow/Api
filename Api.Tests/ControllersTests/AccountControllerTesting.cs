using FluentAssertions;
using Api.Models.Entities;
using Xunit;
using Api.Models;
using Api.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Tests.ControllersTests;

public class AccountControllerTesting
{
    private readonly AccountsController AccountsController;

    public AccountControllerTesting()
    {
        AccountsController = new AccountsController(
            new ModelsContext(
                new DbContextOptionsBuilder<ModelsContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options)
            );
    }

    [Fact]
    public async void CreateAccount_ValidAccount_ShouldReturn_Created()
    {
        ActionResult<AccountResource> request = await AccountsController.Create(new CreateAccount()
        {
            MailAddress = "gtouchet@myges.fr",
            Password = "Pass123!",
        });

        request.Result.GetType().Should().Be(typeof(CreatedResult));
    }

    [Fact]
    public async void CreateAccount_WithoutName_ShouldReturn_Created()
    {
        ActionResult<AccountResource> request = await AccountsController.Create(new CreateAccount()
        {
            MailAddress = "gtouchet@myges.fr",
            Password = "Pass123!",
        });

        request.Result.GetType().Should().Be(typeof(CreatedResult));
    }

    [Fact]
    public async void CreateAccount_WithoutName_ShouldSetDefaultNames()
    {
        ActionResult<AccountResource> request = await AccountsController.Create(new CreateAccount()
        {
            MailAddress = "gtouchet@myges.fr",
            Password = "Pass123!",
        });
        CreatedResult result = request.Result as CreatedResult;
        AccountResource account = result.Value as AccountResource;

        account.Firstname.Should().Be("Unknown");
        account.Lastname.Should().Be("Unknown");
    }

    [Fact]
    public async void CreateAccount_WithoutMailAddress_ShouldReturn_BadRequest()
    {
        ActionResult<AccountResource> request = await AccountsController.Create(new CreateAccount()
        {
            Password = "Pass123!",
        });

        request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
    }

    [Fact]
    public async void CreateAccount_WithoutPassword_ShouldReturn_BadRequest()
    {
        ActionResult<AccountResource> request = await AccountsController.Create(new CreateAccount()
        {
            MailAddress = "gtouchet@myges.fr",
        });

        request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
    }

    [Fact]
    public async void CreateTwoAccounts_WithSameMailAddresses_ShouldReturn_BadRequest()
    {
        await AccountsController.Create(new CreateAccount()
        {
            MailAddress = "gtouchet@myges.fr",
            Password = "Pass123!",
        });

        ActionResult<AccountResource> request = await AccountsController.Create(new CreateAccount()
        {
            MailAddress = "gtouchet@myges.fr",
            Password = "123myPass?",
        });

        request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
    }

    [Fact]
    public async void ReadAllAccounts_Size_ShouldBe_1()
    {
        await AccountsController.Create(new CreateAccount()
        {
            MailAddress = "gtouchet@myges.fr",
            Password = "Pass123!",
        });

        ActionResult<List<AccountResource>> request = await AccountsController.Read();
        OkObjectResult result = request.Result as OkObjectResult;
        List<AccountResource> accounts = result.Value as List<AccountResource>;

        int size = 0;
        accounts.ForEach(account => size += 1);

        size.Should().Be(1);
    }

    [Fact]
    public async void ReadAccount_ExistingAddress_ShouldReturn_Ok()
    {
        ActionResult<AccountResource> request = await AccountsController.Create(new CreateAccount()
        {
            MailAddress = "gtouchet@myges.fr",
            Password = "Pass123!",
        });
        CreatedResult result = request.Result as CreatedResult;
        AccountResource account = result.Value as AccountResource;

        ActionResult<List<AccountResource>> request2 = await AccountsController.Read(account.MailAddress);

        request2.Result.GetType().Should().Be(typeof(OkObjectResult));
    }

    [Fact]
    public async void ReadAccount_RandomAddress_ShouldReturn_NotFound()
    {
        await AccountsController.Create(new CreateAccount()
        {
            MailAddress = "gtouchet@myges.fr",
            Password = "Pass123!",
        });

        ActionResult<List<AccountResource>> request = await AccountsController.Read("random@nope.fr");

        request.Result.GetType().Should().Be(typeof(NotFoundObjectResult));
    }
}
