namespace Api.Tests.HandlersTests.GroupHandlersTests;

public class KickMemberFromGroupCommandTests : TestBase
{
    [Fact(DisplayName =
        "Kicking a member from a group, as the group owner\n" +
        "Should kick the member")]
    public async void KickMemberFromGroupCommandTest_1()
    {
        await this.RegisterNewAccount("groupOwner@myges.fr");
        Group group = await this.RegisterNewGroup("groupOwner@myges.fr");
        Account member = await this.RegisterNewAccount("groupMember@myges.fr");

        await new JoinGroupCommand(this.context).Handle(new JoinGroupDto()
        {
            MailAddress = "groupMember@myges.fr",
            GroupId = group.Id,
        });

        await new KickMemberFromGroupCommand(this.context).Handle(new KickMemberDto()
        {
            CallerMailAddress = "groupOwner@myges.fr",
            MemberMailAddress = "groupMember@myges.fr",
            GroupId = group.Id,
        });

        group.Members.Should().NotContain(member);
    }

    [Fact(DisplayName =
        "Kicking a member from a group, not as the group owner\n" +
        "Should not kick the member\n" +
        "And throw exception with code 403 and error type \"Pas le créateur du groupe\"")]
    public async void KickMemberFromGroupCommandTest_2()
    {
        await this.RegisterNewAccount("groupOwner@myges.fr");
        Group group = await this.RegisterNewGroup("groupOwner@myges.fr");
        Account member_1 = await this.RegisterNewAccount("groupMember_1@myges.fr");
        Account member_2 = await this.RegisterNewAccount("groupMember_2@myges.fr");

        await new JoinGroupCommand(this.context).Handle(new JoinGroupDto()
        {
            MailAddress = "groupMember_1@myges.fr",
            GroupId = group.Id,
        });

        await new JoinGroupCommand(this.context).Handle(new JoinGroupDto()
        {
            MailAddress = "groupMember_2@myges.fr",
            GroupId = group.Id,
        });

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new KickMemberFromGroupCommand(this.context).Handle(new KickMemberDto()
            {
                CallerMailAddress = "groupMember_1@myges.fr",
                MemberMailAddress = "groupMember_2@myges.fr",
                GroupId = group.Id,
            }));

        request.Content.StatusCode.Should().Be(403);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Pas le créateur du groupe");
        group.Members.Should().Contain(member_1);
    }

    [Fact(DisplayName =
        "Kicking the owner of a group\n" +
        "Should throw exception with code 400 and error type \"Impossible de quitter le groupe\"")]
    public async void KickMemberFromGroupCommandTest_3()
    {
        Account owner = await this.RegisterNewAccount("groupOwner@myges.fr");
        Group group = await this.RegisterNewGroup("groupOwner@myges.fr");

        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new KickMemberFromGroupCommand(this.context).Handle(new KickMemberDto()
            {
                CallerMailAddress = "groupOwner@myges.fr",
                MemberMailAddress = "groupOwner@myges.fr",
                GroupId = group.Id,
            }));

        request.Content.StatusCode.Should().Be(400);
        request.Content.Value.As<ExceptionDto>().Error.Should().Be("Impossible de quitter le groupe");
        group.Members.Should().Contain(owner);
    }
}
