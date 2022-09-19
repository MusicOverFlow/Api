namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class ReadAccountByNameQueryTests : TestBase
{
    private Account account;

    public ReadAccountByNameQueryTests()
    {
        this.account = new CreateAccountCommand(this.context, this.container).Handle(new CreateAccountDto()
        {
            MailAddress = "gt@myges.fr",
            Password = "123Pass!",
            Firstname = "Guillaume",
            Lastname = "Touchet",
        }).Result;
    }

    [Fact(DisplayName = "Reading accounts by exact names should return the base account")]
    public void ReadAccountByNameQueryTest_1()
    {
        List<Account> readAccounts = new ReadAccountsByNameQuery(this.context).Handle(new ReadByNamesDto()
        {
            Firstname = this.account.Firstname,
            Lastname = this.account.Lastname,
        }).Result;

        readAccounts.Should().ContainEquivalentOf(this.account);
    }

    [Fact(DisplayName = "Reading accounts by almost same names should return the base account")]
    public void ReadAccountByNameQueryTest_2()
    {
        List<Account> readAccounts = new ReadAccountsByNameQuery(this.context).Handle(new ReadByNamesDto()
        {
            Firstname = "Guill",
            Lastname = "Touch",
        }).Result;

        readAccounts.Should().ContainEquivalentOf(this.account);
    }

    [Fact(DisplayName = "Reading accounts by exact lastname should return the base account")]
    public void ReadAccountByNameQueryTest_3()
    {
        List<Account> readAccounts = new ReadAccountsByNameQuery(this.context).Handle(new ReadByNamesDto()
        {
            Lastname = "Touchet",
        }).Result;

        readAccounts.Should().ContainEquivalentOf(this.account);
    }

    [Fact(DisplayName = "Reading accounts by inexsting names should return an empty list")]
    public void ReadAccountByNameQueryTest_4()
    {
        List<Account> readAccounts = new ReadAccountsByNameQuery(this.context).Handle(new ReadByNamesDto()
        {
            Firstname = "Frédéric",
            Lastname = "Sananes",
        }).Result;

        readAccounts.Should().BeEmpty();
    }
}
