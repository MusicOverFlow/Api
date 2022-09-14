namespace Api.Tests.HandlersTests.GroupHandlersTests;

public class KickMemberFromGroupCommandTests : TestBase
{
    private Account groupOwner, member;
    private Group group;

    public KickMemberFromGroupCommandTests()
    {
        this.groupOwner = this.RegisterNewAccount("gt1@myges.fr").Result;
        this.member = this.RegisterNewAccount("gt2@myges.fr").Result;
        this.group = this.RegisterNewGroup(this.groupOwner.MailAddress).Result;

        _ = new JoinGroupCommand(this.context).Handle(new JoinGroupDto()
        {
            MailAddress = this.member.MailAddress,
            GroupId = this.group.Id,
        });
    }

    [Fact(DisplayName =
        "Kicking a member from a group, as the group owner\n" +
        "Should kick the member")]
    public async void KickMemberFromGroupCommandTest_1()
    {
        await new KickMemberFromGroupCommand(this.context).Handle(new KickMemberDto()
        {
            CallerMailAddress = this.groupOwner.MailAddress,
            MemberMailAddress = this.member.MailAddress,
            GroupId = this.group.Id,
        });
        
        this.group.Members.Should().NotContain(this.member);
    }

    [Fact(DisplayName =
        "Kicking a member from a group, not as the group owner\n" +
        "Should not kick the member\n" +
        "And throw exception code 403")]
    public async void KickMemberFromGroupCommandTest_2()
    {
        Account otherMember = await this.RegisterNewAccount("gt3@myges.fr");
        
        await new JoinGroupCommand(this.context).Handle(new JoinGroupDto()
        {
            MailAddress = otherMember.MailAddress,
            GroupId = this.group.Id,
        });

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new KickMemberFromGroupCommand(this.context).Handle(new KickMemberDto()
            {
                CallerMailAddress = this.member.MailAddress,
                MemberMailAddress = otherMember.MailAddress,
                GroupId = this.group.Id,
            }));

        request.Content.StatusCode.Should().Be(403);
        this.group.Members.Should().Contain(this.member);
    }

    [Fact(DisplayName =
        "Kicking the owner of a group\n" +
        "Should throw exception code 400")]
    public async void KickMemberFromGroupCommandTest_3()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new KickMemberFromGroupCommand(this.context).Handle(new KickMemberDto()
            {
                CallerMailAddress = this.groupOwner.MailAddress,
                MemberMailAddress = this.groupOwner.MailAddress,
                GroupId = this.group.Id,
            }));

        request.Content.StatusCode.Should().Be(400);
        this.group.Members.Should().Contain(this.groupOwner);
    }
}
