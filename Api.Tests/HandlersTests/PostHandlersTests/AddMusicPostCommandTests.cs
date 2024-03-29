﻿using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;

namespace Api.Tests.HandlersTests.PostHandlersTests;

public class AddMusicPostCommandTests : TestBase
{
    private Account account;
    private Post post;

    public AddMusicPostCommandTests()
    {
        this.account = this.RegisterNewAccount("gr@myges.fr").Result;
        this.post = this.RegisterNewPost(this.account.MailAddress).Result;
    }
    
    [Fact(DisplayName =
        "Adding a sound with valid format to a post\n" +
        "Should update the post's sound URL")]
    public async void AddMusicPostHandlerTest_1()
    {
        byte[] fakeSound = new byte[] { 0, 1, 2, 3, 4 };

        Post postWithMusic = await new AddMusicPostCommand(this.context, this.container).Handle(new AddMusicDto()
        {
            PostId = this.post.Id,
            File = new FormFile(
                baseStream: new MemoryStream(fakeSound),
                baseStreamOffset: 0,
                length: fakeSound.Length,
                name: "",
                fileName: "mySound.mp3"),
        });

        postWithMusic.MusicUrl.Should().Be($"https://post-sounds.s3.eu-west-3.amazonaws.com/{postWithMusic.Id}.mySound.mp3");

        await this.container.DeletePostSound($"{postWithMusic.Id}.mySound.mp3");
    }

    [Fact(DisplayName =
        "Adding a sound with valid format to a post\n" +
        "Should update the post's sound on AWS")]
    public async void AddMusicPostHandlerTest_2()
    {
        byte[] fakeSound = new byte[] { 0, 1, 2, 3, 4 };

        Post postWithSound = await new AddMusicPostCommand(this.context, this.container).Handle(new AddMusicDto()
        {
            PostId = this.post.Id,
            File = new FormFile(
                baseStream: new MemoryStream(fakeSound),
                baseStreamOffset: 0,
                length: fakeSound.Length,
                name: "",
                fileName: "mySound.mp3"),
        });

        // Download the file as bytes and compare it to the fake sound
        using HttpClient client = new HttpClient();
        using HttpResponseMessage response = await client.GetAsync(postWithSound.MusicUrl);
        using Stream stream = await response.Content.ReadAsStreamAsync();
        byte[] downloadedFile = new byte[fakeSound.Length];
        stream.Read(downloadedFile, 0, fakeSound.Length);
        downloadedFile.Should().Equal(fakeSound);

        await this.container.DeletePostSound($"{postWithSound.Id}.mySound.mp3");
    }
}
