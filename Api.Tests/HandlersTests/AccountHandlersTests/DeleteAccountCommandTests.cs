﻿namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class DeleteAccountCommandTests : TestBase
{
    [Fact(DisplayName =
        "Deleting an existing account\n" +
        "Should delete the account")]
    public async void DeleteAccountHandlerTest_1()
    {
        await this.RegisterNewAccount("gt@myges.fr");

        await new DeleteAccountCommand(this.context).Handle("gt@myges.fr");

        List<Account> accounts = await new ReadAccountByMailQuery(this.context).Handle();
        accounts.Count.Should().Be(0);
    }

    [Fact(DisplayName =
        "Deleting an unexisting account\n" +
        "Should throw exception with code 404 and error type \"Compte introuvable\"")]
    public async void DeleteAccountHandlerTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new DeleteAccountCommand(this.context).Handle("gt@myges.fr"));

        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Compte introuvable");
    }
}