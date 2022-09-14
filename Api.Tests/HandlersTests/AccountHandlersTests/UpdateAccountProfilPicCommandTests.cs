using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;

namespace Api.Tests.HandlersTests.AccountHandlersTests;

public class UpdateAccountProfilPicCommandTests : TestBase
{
    private Account account;

    public UpdateAccountProfilPicCommandTests()
    {
        this.account = this.RegisterNewAccount("gt@myges.fr").Result;
    }
    
    [Fact(DisplayName =
        "Updating an account with an invalid profil pic file format\n" +
        "Should throw exception code 400")]
    public async void UpdateAccountProfilPicHandlerTest_1()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountProfilPicCommand(this.context, this.container).Handle(new UpdateProfilPicDto()
            {
                MailAddress = this.account.MailAddress,
                ProfilPic = new FormFile(
                    baseStream: null,
                    baseStreamOffset: 0,
                    length: 1,
                    name: "",
                    fileName: "myProfilPic.mp3"),
            }));

        request.Content.StatusCode.Should().Be(400);
    }

    [Fact(DisplayName =
        "Updating an inexisting account's profil pic\n" +
        "Should throw exception code 404")]
    public async void UpdateAccountProfilPicHandlerTest_2()
    {
        HandlerException request = await Assert.ThrowsAsync<HandlerException>(
            () => new UpdateAccountProfilPicCommand(this.context, this.container).Handle(new UpdateProfilPicDto()
            {
                MailAddress = "unkown@myges.fr",
                ProfilPic = new FormFile(
                    baseStream: null,
                    baseStreamOffset: 0,
                    length: 1,
                    name: "",
                    fileName: "myProfilPic.png"),
            }));

        request.Content.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName =
        "Updating an account with a valid file\n" +
        "Should update the account's profil pic URL")]
    public async void UpdateAccountProfilPicHandlerTest_3()
    {
        Account account = await this.RegisterNewAccount(this.fakeAccountForAwsTesting);

        byte[] fakeImage = new byte[] { 0, 1, 2, 3, 4 };

        account = await new UpdateAccountProfilPicCommand(this.context, this.container).Handle(new UpdateProfilPicDto()
        {
            MailAddress = account.MailAddress,
            ProfilPic = new FormFile(
                baseStream: new MemoryStream(fakeImage),
                baseStreamOffset: 0,
                length: fakeImage.Length,
                name: "",
                fileName: "myProfilPic.png"),
        });

        account.PicUrl.Should().Contain($"{account.MailAddress}.png");

        await this.container.DeleteAccountPic(account.MailAddress);
    }

    [Fact(DisplayName =
        "Updating an account with a valid file\n" +
        "Should update the account's profil pic on AWS")]
    public async void UpdateAccountProfilPicHandlerTest_4()
    {
        Account account = await this.RegisterNewAccount(this.fakeAccountForAwsTesting);

        byte[] fakeImage = new byte[] { 0, 1, 2, 3, 4 };

        account = await new UpdateAccountProfilPicCommand(this.context, this.container).Handle(new UpdateProfilPicDto()
        {
            MailAddress = account.MailAddress,
            ProfilPic = new FormFile(
                baseStream: new MemoryStream(fakeImage),
                baseStreamOffset: 0,
                length: fakeImage.Length,
                name: "",
                fileName: "myProfilPic.png"),
        });

        // Download the file as bytes and compare it to the fake image
        using HttpClient client = new HttpClient();
        using HttpResponseMessage response = await client.GetAsync(account.PicUrl);
        using Stream stream = await response.Content.ReadAsStreamAsync();
        byte[] downloadedFile = new byte[fakeImage.Length];
        stream.Read(downloadedFile, 0, fakeImage.Length);
        downloadedFile.Should().Equal(fakeImage);

        await this.container.DeleteAccountPic(account.MailAddress);
    }
}
