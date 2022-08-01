using Azure.Storage.Blobs;

namespace Api.Utilitaries;

public class Blob
{
    private const string PROFIL_PICS = "profil-pics";
    private const string GROUP_PICS = "group-pics";
    private const string POST_SOUNDS = "music-storage";
    private const string PIPELINE_IMAGES = "pipeline-images";
    private const string PIPELINE_SOUNDS = "pipeline-sounds";
    private const string POST_SCRIPTS = "post-scripts";

    private readonly string azureContainerBaseUrl;

    public Blob(IConfiguration configuration)
    {
        this.azureContainerBaseUrl = configuration.GetSection("ConnectionStrings:MusicOverflowStorageAccount").Value;
    }
    
    public async Task<string> GetProfilPicUrl(byte[] profilPic, string mailAddress)
    {
        if (profilPic == null)
        {
            return $"https://musicoverflowstorage.blob.core.windows.net/{PROFIL_PICS}/placeholder.png";
        }
        
        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerBaseUrl, PROFIL_PICS);
        BlobClient blobClient = blobContainer.GetBlobClient($"{mailAddress}.png");
        await blobClient.UploadAsync(new BinaryData(profilPic), overwrite: true);
        return blobClient.Uri.AbsoluteUri.Replace("%40", "@");
    }

    public async Task<string> GetGroupPicUrl(byte[] groupPic, Guid groupId)
    {
        if (groupPic == null)
        {
            return $"https://musicoverflowstorage.blob.core.windows.net/{GROUP_PICS}/placeholder.png";
        }
        
        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerBaseUrl, GROUP_PICS);
        BlobClient blobClient = blobContainer.GetBlobClient($"{groupId}.png");
        await blobClient.UploadAsync(new BinaryData(groupPic), overwrite: true);
        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> GetMusicUrl(byte[] sound, Guid postId, string filename)
    {
        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerBaseUrl, POST_SOUNDS);
        BlobClient blobClient = blobContainer.GetBlobClient($"{postId}.{filename}");
        await blobClient.UploadAsync(new BinaryData(sound), overwrite: true);
        return blobClient.Uri.AbsoluteUri;
    }
    
    // TODO: expiration du fichier pour nettoyage automatique -> visiblement impossible
    public async Task<string> GetPipelineImageUrl(byte[] image, string filename)
    {
        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerBaseUrl, PIPELINE_IMAGES);
        BlobClient blobClient = blobContainer.GetBlobClient($"{Guid.NewGuid()}.{filename}");
        await blobClient.UploadAsync(new BinaryData(image));
        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> GetPipelineSoundUrl(byte[] sound, string filename)
    {
        BlobContainerClient blobContainer = new BlobContainerClient(this.azureContainerBaseUrl, PIPELINE_SOUNDS);
        BlobClient blobClient = blobContainer.GetBlobClient($"{Guid.NewGuid()}.{filename}");
        await blobClient.UploadAsync(new BinaryData(sound));
        return blobClient.Uri.AbsoluteUri;
    }
}
