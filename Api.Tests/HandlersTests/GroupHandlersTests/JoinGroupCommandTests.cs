namespace Api.Tests.HandlersTests.GroupHandlersTests;

public class JoinGroupCommandTests : TestBase
{
    private Account groupOwner, member;
    private Group group;

    public JoinGroupCommandTests()
    {
        this.groupOwner = this.RegisterNewAccount("gt1@myges.fr").Result;
        this.member = this.RegisterNewAccount("gt2@myges.fr").Result;
        this.group = this.RegisterNewGroup(this.groupOwner.MailAddress).Result;
    }

    [Fact(DisplayName =
        "Joining a group should add the account to the group's members")]
    public async void JoinGroupCommandTest_1()
    {
        await new JoinGroupCommand(this.context).Handle(new JoinGroupDto()
        {
            MailAddress = this.member.MailAddress,
            GroupId = this.group.Id,
        });

        // 2 because the creator is already in the group
        this.group.Members.Should().HaveCount(2);
    }

    [Fact(DisplayName =
        "Joining an inexisting group\n" +
        "Should not add the account to the group's members\n" +
        "And throw exception code 404")]
    public async void JoinGroupCommandTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new JoinGroupCommand(this.context).Handle(new JoinGroupDto()
            {
                MailAddress = this.member.MailAddress,
                GroupId = Guid.NewGuid(),
            }));
        
        this.member.Groups.Should().BeEmpty();
        request.Content.StatusCode.Should().Be(404);
    }
}
