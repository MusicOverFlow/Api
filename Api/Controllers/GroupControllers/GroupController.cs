using Azure.Storage.Blobs;

namespace Api.Controllers.GroupControllers;

[ApiController]
[Route("api/groups")]
public partial class GroupController : ControllerBase
{
    private readonly int MAX_GROUPS_IN_SEARCHES = 20;

    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly IConfiguration configuration;
    private readonly LevenshteinDistance stringComparer;
    private readonly ExceptionHandler exceptionHandler;

    public GroupController(
        ModelsContext context,
        Mapper mapper,
        IConfiguration configuration,
        LevenshteinDistance stringComparer,
        ExceptionHandler exceptionHandler)
    {
        this.context = context;
        this.mapper = mapper;
        this.configuration = configuration;
        this.stringComparer = stringComparer;
        this.exceptionHandler = exceptionHandler;
    }

    async private Task<string> GetGroupPicUrl(byte[] groupPic)
    {
        if (groupPic != null)
        {
            BlobContainerClient blobContainer = new BlobContainerClient(
                this.configuration.GetSection("ConnectionStrings:MusicOverflowStorageAccount").Value,
                "group-pics"
            );

            BlobClient blobClient = blobContainer.GetBlobClient(Guid.NewGuid() + ".group.png");
            await blobClient.UploadAsync(new BinaryData(groupPic), overwrite: true);

            return blobClient.Uri.AbsoluteUri;
        }
        else
        {
            return "https://musicoverflowstorage.blob.core.windows.net/group-pics/placeholder.group.png";
        }
    }
}
