using FluentAssertions;
using Api.Models.Entities;
using Xunit;
using Api.Models;
using Api.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Tests
{
    public class AccountControllerTesting
    {
        private readonly AccountsController AccountsController;

        public AccountControllerTesting()
        {
            this.AccountsController = new AccountsController(
                new ModelsContext(
                    new DbContextOptionsBuilder<ModelsContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options)
                );
        }

        /**
         * Account creation testing
         */
        [Fact]
        public async void CreateAccount_ValidAccount_ShouldReturn_Created()
        {
            ActionResult<AccountResource> request = await this.AccountsController.Create(new CreateAccount()
            {
                MailAddress = "gtouchet@myges.fr",
                Password = "Pass123!",
            });

            request.Result.GetType().Should().Be(typeof(CreatedResult));
        }

        [Fact]
        public async void CreateAccount_WithoutName_ShouldReturn_Created()
        {
            ActionResult<AccountResource> request = await this.AccountsController.Create(new CreateAccount()
            {
                MailAddress = "gtouchet@myges.fr",
                Password = "Pass123!",
            });

            request.Result.GetType().Should().Be(typeof(CreatedResult));
        }

        [Fact]
        public async void CreateAccount_WithoutName_ShouldSetDefaultNames()
        {
            ActionResult<AccountResource> request = await this.AccountsController.Create(new CreateAccount()
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
            ActionResult<AccountResource> request = await this.AccountsController.Create(new CreateAccount()
            {
                Password = "Pass123!",
            });

            request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async void CreateAccount_InvalidMailAddress_ShouldReturn_BadRequest()
        {
            ActionResult<AccountResource> request = await this.AccountsController.Create(new CreateAccount()
            {
                MailAddress = "", // TODO: mettre une addresse invalide quand on aura implémenté le NuGet de validation d'email
                Password = "Pass123!",
            });

            request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async void CreateAccount_WithoutPassword_ShouldReturn_BadRequest()
        {
            ActionResult<AccountResource> request = await this.AccountsController.Create(new CreateAccount()
            {
                MailAddress = "gtouchet@myges.fr",
            });

            request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async void CreateAccount_InvalidPassword_ShouldReturn_BadRequest()
        {
            ActionResult<AccountResource> request = await this.AccountsController.Create(new CreateAccount()
            {
                MailAddress = "gtouchet@myges.fr",
                Password = "", // TODO: mettre un mot de passe invalide quand on aura décidé d'une convention
            });

            request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async void CreateTwoAccounts_WithSameMailAddresses_ShouldReturn_BadRequest()
        {
            await this.AccountsController.Create(new CreateAccount()
            {
                MailAddress = "gtouchet@myges.fr",
                Password = "Pass123!",
            });

            ActionResult<AccountResource> request = await this.AccountsController.Create(new CreateAccount()
            {
                MailAddress = "gtouchet@myges.fr",
                Password = "123myPass?",
            });

            request.Result.GetType().Should().Be(typeof(BadRequestObjectResult));
        }

        /**
         * Account read testing
         */
        [Fact]
        public async void GetAllAccounts_Size_ShouldBe_1()
        {
            await this.AccountsController.Create(new CreateAccount()
            {
                MailAddress = "gtouchet@myges.fr",
                Password = "Pass123!",
            });

            ActionResult<List<AccountResource>> request = await this.AccountsController.Read();
            OkObjectResult result = request.Result as OkObjectResult;
            List<AccountResource> accounts = result.Value as List<AccountResource>;

            int size = 0;
            accounts.ForEach(account => size += 1);

            size.Should().Be(1);
        }
        
        [Fact]
        public async void GetAccountByItsId_ValidId_ShouldReturn_Ok()
        {
            ActionResult<AccountResource> request = await this.AccountsController.Create(new CreateAccount()
            {
                MailAddress = "gtouchet@myges.fr",
                Password = "Pass123!",
            });
            CreatedResult result = request.Result as CreatedResult;
            AccountResource account = result.Value as AccountResource;

            ActionResult<List<AccountResource>> request2 = await this.AccountsController.Read(account.MailAddress);

            request2.Result.GetType().Should().Be(typeof(OkObjectResult));
        }

        [Fact]
        public async void GetAccountByItsId_RandomId_ShouldReturn_NotFound()
        {
            await this.AccountsController.Create(new CreateAccount()
            {
                MailAddress = "gtouchet@myges.fr",
                Password = "Pass123!",
            });

            ActionResult<List<AccountResource>> request = await this.AccountsController.Read("random@nope.fr");

            request.Result.GetType().Should().Be(typeof(NotFoundObjectResult));
        }
    }
}