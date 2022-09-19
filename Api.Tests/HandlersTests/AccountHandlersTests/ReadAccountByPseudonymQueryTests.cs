namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class ReadAccountByPseudonymQueryTests : TestBase
{
    private Account account;

    public ReadAccountByPseudonymQueryTests()
    {
        this.account = new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
        {
            MailAddress = "gt@myges.fr",
            Password = "123Pass!",
            Pseudonym = "gtouchet",
        }).Result;
    }

    [Fact(DisplayName = "Reading accounts by exact pseudonym should return the base account")]
    public void ReadAccountByPseudonymQueryTest_1()
    {
        List<Account> readAccounts = new ReadAccountByPseudonymQuery(this.context).Handle(this.account.Pseudonym).Result;
        readAccounts.Should().ContainEquivalentOf(this.account);
    }

    [Fact(DisplayName = "Reading accounts by almost same pseudonym should return the base account")]
    public void ReadAccountByPseudonymQueryTest_2()
    {
        List<Account> readAccounts = new ReadAccountByPseudonymQuery(this.context).Handle("gtouc").Result;
        readAccounts.Should().ContainEquivalentOf(this.account);
    }

    [Fact(DisplayName = "Reading accounts by inexisting pseudonym should return an empty list")]
    public void ReadAccountByPseudonymQueryTest_3()
    {
        List<Account> readAccounts = new ReadAccountByPseudonymQuery(this.context).Handle("fred").Result;
        readAccounts.Should().BeEmpty();
    }
}
