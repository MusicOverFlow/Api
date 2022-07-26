using Azure.Storage.Blobs;
using System.Security.Cryptography;

namespace Api.Controllers.AccountControllers;

[ApiController]
[Route("api/accounts")]
public partial class AccountController : ControllerBase
{
    private readonly int MAX_ACCOUNTS_IN_SEARCHES = 20;

    private readonly ModelsContext context;
    private readonly Mapper mapper;
    private readonly DataValidator dataValidator;
    private readonly IConfiguration configuration;
    private readonly LevenshteinDistance stringComparer;
    private readonly ExceptionHandler exceptionHandler;

    public AccountController(
        ModelsContext context,
        Mapper mapper,
        DataValidator dataValidator,
        IConfiguration configuration,
        LevenshteinDistance stringComparer,
        ExceptionHandler exceptionHandler)
    {
        this.context = context;
        this.mapper = mapper;
        this.dataValidator = dataValidator;
        this.configuration = configuration;        
        this.stringComparer = stringComparer;
        this.exceptionHandler = exceptionHandler;
    }

    async private Task<string> GetProfilPicUrl(byte[] profilPic, string mailAddress)
    {
        if (profilPic != null)
        {
            BlobContainerClient blobContainer = new BlobContainerClient(
                this.configuration.GetSection("ConnectionStrings:MusicOverflowStorageAccount").Value,
                "profil-pics"
            );

            BlobClient blobClient = blobContainer.GetBlobClient(mailAddress + ".profilpic.png");
            await blobClient.UploadAsync(new BinaryData(profilPic), overwrite: true);

            return blobClient.Uri.AbsoluteUri.Replace("%40", "@");
        }
        else
        {
            return "https://musicoverflowstorage.blob.core.windows.net/profil-pics/placeholder.profilpic.png";
        }
    }

    private void EncryptPassword(string password, out byte[] hash, out byte[] salt)
    {
        using HMACSHA512 hmac = new HMACSHA512();

        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        salt = hmac.Key;
    }
}
