using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;

namespace Api.Tests.HandlersTests.PostHandlersTests;

public class AddMusicPostCommandTests : TestBase
{
    [Fact(DisplayName =
        "Adding a sound with valid format to a post\n" +
        "Should update the post's sound URL")]
    public async void AddMusicPostHandlerTest_1()
    {
        await this.RegisterNewAccount("gt@myges.fr");
        Post post = await this.RegisterNewPost("gt@myges.fr");

        byte[] fakeSound = new byte[] { 0, 1, 2, 3, 4 };

        post = await new AddMusicPostCommand(this.context).Handle(new AddMusicDto()
        {
            PostId = post.Id,
            File = new FormFile(
                baseStream: new MemoryStream(fakeSound),
                baseStreamOffset: 0,
                length: fakeSound.Length,
                name: "",
                fileName: "mySound.mp3"),
        });
        
        post.MusicUrl.Should().Be($"https://post-sounds.s3.eu-west-3.amazonaws.com/{post.Id}.mySound.mp3");

        await Blob.DeletePostSound($"{post.Id}.mySound.mp3");
    }

    [Fact(DisplayName =
        "Adding a sound with valid format to a post\n" +
        "Should update the post's sound on AWS")]
    public async void AddMusicPostHandlerTest_2()
    {
        await this.RegisterNewAccount("gt@myges.fr");
        Post postWithoutSound = await this.RegisterNewPost("gt@myges.fr");

        byte[] fakeSound = new byte[] { 0, 1, 2, 3, 4 };

        Post postWithSound = await new AddMusicPostCommand(this.context).Handle(new AddMusicDto()
        {
            PostId = postWithoutSound.Id,
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

        await Blob.DeletePostSound($"{postWithSound.Id}.mySound.mp3");
    }
}
