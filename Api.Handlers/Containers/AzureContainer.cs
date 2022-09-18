using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace Api.Handlers.Containers;

public class AzureContainer : ContainerNames, IContainer
{
    private readonly string azureContainerConnectionString;

    public AzureContainer(IConfiguration configuration)
    {
        this.azureContainerConnectionString = configuration.GetSection("ConnectionStrings:MusicOverflowStorageAccount").Value;
    }

    public async Task<string> GetProfilPicUrl(byte[] profilPic, string mailAddress)
    {
        if (profilPic == null || profilPic.Length == 0)
        {
            return $"https://musicoverflowstorage.blob.core.windows.net/{PROFIL_PICS}/placeholder.png";
        }

        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerConnectionString, PROFIL_PICS);
        BlobClient blobClient = blobContainer.GetBlobClient($"{mailAddress}.png");
        await blobClient.UploadAsync(new BinaryData(profilPic), overwrite: true);
        return blobClient.Uri.AbsoluteUri.Replace("%40", "@");
    }

    public async Task<string> GetGroupPicUrl(byte[] groupPic, Guid groupId)
    {
        if (groupPic == null || groupPic.Length == 0)
        {
            return $"https://musicoverflowstorage.blob.core.windows.net/{GROUP_PICS}/placeholder.png";
        }

        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerConnectionString, GROUP_PICS);
        BlobClient blobClient = blobContainer.GetBlobClient($"{groupId}.png");
        await blobClient.UploadAsync(new BinaryData(groupPic), overwrite: true);
        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> GetMusicUrl(byte[] sound, Guid postId, string filename)
    {
        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerConnectionString, POST_SOUNDS);
        BlobClient blobClient = blobContainer.GetBlobClient($"{postId}.{filename}");
        await blobClient.UploadAsync(new BinaryData(sound), overwrite: true);
        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> GetPipelineSoundUrl(byte[] sound, string filename)
    {
        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerConnectionString, PIPELINE_SOUNDS);
        BlobClient blobClient = blobContainer.GetBlobClient(filename);
        await blobClient.UploadAsync(new BinaryData(sound), overwrite: true);
        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> GetPostScriptUrl(string script, Guid postId)
    {
        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerConnectionString, POST_SCRIPTS);
        BlobClient blobClient = blobContainer.GetBlobClient(postId.ToString());
        await blobClient.UploadAsync(new BinaryData(script), overwrite: true);
        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> GetConvertedSoundUrl(byte[] sound, string filename)
    {
        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerConnectionString, CONVERTED_SOUNDS);
        BlobClient blobClient = blobContainer.GetBlobClient(filename);
        await blobClient.UploadAsync(new BinaryData(sound), overwrite: true);
        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> GetPipelineImageUrl(byte[] image, string filename)
    {
        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerConnectionString, PIPELINE_IMAGES);
        BlobClient blobClient = blobContainer.GetBlobClient(filename);
        await blobClient.UploadAsync(new BinaryData(image), overwrite: true);
        return blobClient.Uri.AbsoluteUri;
    }

    public Task DeletePostScript(Guid postId)
    {
        throw new NotImplementedException();
    }

    public Task DeletePostSound(string file)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAccountPic(string mailAddress)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGroupPic(Guid groupId)
    {
        throw new NotImplementedException();
    }
}
