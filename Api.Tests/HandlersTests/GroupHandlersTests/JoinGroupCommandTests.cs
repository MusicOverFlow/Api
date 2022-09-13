using Microsoft.EntityFrameworkCore;

namespace Api.Tests.HandlersTests.GroupHandlersTests;

public class JoinGroupCommandTests : TestBase
{
    [Fact(DisplayName =
        "Joining a group should add the account to the group's members")]
    public async void JoinGroupCommandTest_1()
    {
        await this.RegisterNewAccount("gt@myges.fr");
        Group group = await this.RegisterNewGroup("gt@myges.fr");
        await this.RegisterNewAccount("newMember@myges.fr");

        await new JoinGroupCommand(this.context).Handle(new JoinGroupDto()
        {
            MailAddress = "newMember@myges.fr",
            GroupId = group.Id,
        });

        // 2 because the creator is already in the group
        group.Members.Should().HaveCount(2);
    }

    [Fact(DisplayName =
        "Joining an inexisting group\n" +
        "Should not add the account to the group's members\n" +
        "And throw exception code 404 with type \"Groupe introuvable\"")]
    public async void JoinGroupCommandTest_2()
    {
        Account account = await this.RegisterNewAccount("newMember@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new JoinGroupCommand(this.context).Handle(new JoinGroupDto()
            {
                MailAddress = "newMember@myges.fr",
                GroupId = Guid.NewGuid(),
            }));
        
        this.context.Accounts.Include(a => a.Groups).Where(a => a.Id.Equals(account.Id)).First().Groups.Should().BeEmpty();
        request.Content.StatusCode.Should().Be(404);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Groupe introuvable");
    }
}
