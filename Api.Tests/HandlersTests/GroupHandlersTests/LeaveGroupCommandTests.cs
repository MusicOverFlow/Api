namespace Api.Tests.HandlersTests.GroupHandlersTests;

public class LeaveGroupCommandTests : TestBase
{
    private Account groupOwner, member, notMember;
    private Group group;
    
    public LeaveGroupCommandTests()
    {
        this.member = this.RegisterNewAccount("member@myges.fr").Result;
        this.notMember = this.RegisterNewAccount("notMember@myges.fr").Result;
        this.groupOwner = this.RegisterNewAccount("gt@myges.fr").Result;
        this.group = this.RegisterNewGroup(this.groupOwner.MailAddress).Result;
        _ = new JoinGroupCommand(this.context).Handle(new JoinGroupDto()
        {
            MailAddress = this.member.MailAddress,
            GroupId = this.group.Id,
        });
    }

    [Fact(DisplayName = "A member leaving a group should remove the account from group's members")]
    public async void LeaveGroupCommandTest_1()
    {
        await new LeaveGroupCommand(this.context).Handle(new LeaveGroupDto()
        {
            MailAddress = this.member.MailAddress,
            GroupId = this.group.Id,
        });

        this.group.Members.Should().NotContainEquivalentOf(this.member);
    }

    [Fact(DisplayName = "A non member leaving a group should not do anything")]
    public async void LeaveGroupCommandTest_2()
    {
        await new LeaveGroupCommand(this.context).Handle(new LeaveGroupDto()
        {
            MailAddress = this.notMember.MailAddress,
            GroupId = this.group.Id,
        });

        this.group.Members.Should().NotContainEquivalentOf(this.notMember);
    }

    [Fact(DisplayName = "Leaving an inexisting group should return error code 404")]
    public async void LeaveGroupCommandTest_3()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new LeaveGroupCommand(this.context).Handle(new LeaveGroupDto()
            {
                MailAddress = this.member.MailAddress,
                GroupId = Guid.NewGuid(),
            }));

        request.Content.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "Leaving a group as the owner should return error code 400")]
    public async void LeaveGroupCommandTest_4()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new LeaveGroupCommand(this.context).Handle(new LeaveGroupDto()
            {
                MailAddress = this.groupOwner.MailAddress,
                GroupId = this.group.Id,
            }));

        request.Content.StatusCode.Should().Be(400);
    }
}
