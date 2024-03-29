﻿using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;

namespace Api.Tests.HandlersTests.GroupHandlersTests;

public class CreateGroupCommandTests : TestBase
{
    private Account account;

    public CreateGroupCommandTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
    }
    
    [Fact(DisplayName =
        "Group creation with a valid request\n" +
        "Shoud create the account")]
    public async void CreateGroupCommandTest_1()
    {
        Group group = await new CreateGroupCommand(this.context, this.container).Handle(new CreateGroupDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Name = "Group name",
        });

        group.Should().NotBeNull();
    }

    [Fact(DisplayName =
        "Group creation without a name\n" +
        "Shoud not create the account\n" +
        "And throw exception code 400")]
    public async void CreateGroupCommandTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateGroupCommand(this.context, this.container).Handle(new CreateGroupDto()
            {
                CreatorMailAddress = this.account.MailAddress,
                Name = null,
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Group creation with an inexisting creator account\n" +
        "Shoud not create the account\n" +
        "And throw exception code 404")]
    public async void CreateGroupCommandTest_3()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new CreateGroupCommand(this.context, this.container).Handle(new CreateGroupDto()
            {
                CreatorMailAddress = "unknown@myges.fr",
                Name = "Group name",
            }));

        request.Content.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName =
        "Group creation with a group pic\n" +
        "Shoud upload the pic on AWS")]
    public async void CreateGroupCommandTest_4()
    {
        byte[] fakeImage = new byte[] { 0, 1, 2, 3, 4 };

        await new CreateGroupCommand(this.context, this.container).Handle(new CreateGroupDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Name = "Group name",
            GroupPic = new FormFile(
                baseStream: new MemoryStream(fakeImage),
                baseStreamOffset: 0,
                length: fakeImage.Length,
                name: "paul",
                fileName: "paul.png"),
        });

        Group group = this.context.Groups.FirstOrDefault(g => g.Name.Equals("Group name"));

        // Download the file as bytes and compare it to the fake image
        using HttpClient client = new HttpClient();
        using HttpResponseMessage response = await client.GetAsync(group.PicUrl);
        using Stream stream = await response.Content.ReadAsStreamAsync();
        byte[] downloadedFile = new byte[fakeImage.Length];
        stream.Read(downloadedFile, 0, fakeImage.Length);
        downloadedFile.Should().Equal(fakeImage);

        await this.container.DeleteGroupPic(group.Id);
    }

    [Fact(DisplayName =
        "Group creation with a group pic\n" +
        "Shoud create the group pic URL")]
    public async void CreateGroupCommandTest_5()
    {
        byte[] fakeImage = new byte[] { 0, 1, 2, 3, 4 };

        Group group = await new CreateGroupCommand(this.context, this.container).Handle(new CreateGroupDto()
        {
            CreatorMailAddress = this.account.MailAddress,
            Name = "Group name",
            GroupPic = new FormFile(
                baseStream: new MemoryStream(fakeImage),
                baseStreamOffset: 0,
                length: fakeImage.Length,
                name: "",
                fileName: "myGroupPic.png"),
        });

        group.PicUrl.Should().Be($"https://group-pics.s3.eu-west-3.amazonaws.com/{group.Id}.png");
        
        await this.container.DeleteGroupPic(group.Id);
    }
}