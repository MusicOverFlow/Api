using Azure.Storage.Blobs;

namespace Api.Controllers.PostControllers;

[ApiController]
[Route("api/posts")]
public partial class PostController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly IConfiguration configuration;
    private readonly ExceptionHandler exceptionHandler;

    public PostController(
        ModelsContext context,
        Mapper mapper,
        IConfiguration configuration,
        ExceptionHandler exceptionHandler)
    {
        this.context = context;
        this.mapper = mapper;
        this.configuration = configuration;
        this.exceptionHandler = exceptionHandler;
    }

    private bool Contains(List<PostResource> posts, Guid id)
    {
        bool result = false;
        posts.ForEach(p =>
        {
            if (p.Id.Equals(id)) result = true;
        });
        return result;
    }

    private async Task<string> GetMusicUrl(byte[] music, Guid postId, string filename)
    {
        BlobContainerClient blobContainer = new BlobContainerClient(
                this.configuration.GetSection("ConnectionStrings:MusicOverflowStorageAccount").Value,
                "music-storage"
            );

        BlobClient blobClient = blobContainer.GetBlobClient($"post.{postId}.{filename}");
        await blobClient.UploadAsync(new BinaryData(music), overwrite: true);

        return blobClient.Uri.AbsoluteUri;
    }
}
