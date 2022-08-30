using Amazon;
using Amazon.S3.Model;
using System.Text;

namespace Api.Utilitaries;

public abstract class Blob
{
    private const string PROFIL_PICS = "profil-pics";
    private const string GROUP_PICS = "group-pics";
    private const string POST_SOUNDS = "post-sounds";
    private const string PIPELINE_IMAGES = "pipeline-images-mo";
    private const string PIPELINE_SOUNDS = "pipeline-sounds";
    private const string POST_SCRIPTS = "post-scripts";
    private const string CONVERTED_SOUNDS = "converted-sounds-mo";

    private readonly static IAmazonS3 s3Client = new AmazonS3Client(
                "AKIA2MFDW34EK2KXP55V", //builder.Configuration.GetSection("AWSClientCredentials:awsAccessKeyId").Value,
                "UMoDXGbIZ615GwF6UeIcERDze+8YC4/8Iczo+tEE", //builder.Configuration.GetSection("AWSClientCredentials:awsSecretAccessKey").Value,
                RegionEndpoint.EUWest3);
    
    public async static Task<string> GetProfilPicUrl(byte[] profilPic, string mailAddress)
    {
        if (profilPic == null)
        {
            return $"https://{PROFIL_PICS}.s3.eu-west-3.amazonaws.com/profil.placeholder.png";
        }

        var request = new PutObjectRequest()
        {
            BucketName = PROFIL_PICS,
            Key = $"{mailAddress}.png",
            InputStream = new MemoryStream(profilPic),
        };
        await s3Client.PutObjectAsync(request);
        return $"https://{PROFIL_PICS}.s3.eu-west-3.amazonaws.com/{mailAddress}.png";
    }

    public async static Task<string> GetGroupPicUrl(byte[] groupPic, Guid groupId)
    {
        if (groupPic == null)
        {
            return $"https://{GROUP_PICS}.s3.eu-west-3.amazonaws.com/group.placeholder.png";
        }

        var request = new PutObjectRequest()
        {
            BucketName = GROUP_PICS,
            Key = $"{groupId}.png",
            InputStream = new MemoryStream(groupPic),
        };
        await s3Client.PutObjectAsync(request);
        return $"https://{PROFIL_PICS}.s3.eu-west-3.amazonaws.com/{groupId}.png";
    }

    public async static Task<string> GetMusicUrl(byte[] sound, Guid postId, string filename)
    {
        var request = new PutObjectRequest()
        {
            BucketName = POST_SOUNDS,
            Key = $"{postId}.{filename}",
            InputStream = new MemoryStream(sound),
        };
        await s3Client.PutObjectAsync(request);
        return $"https://{POST_SOUNDS}.s3.eu-west-3.amazonaws.com/{postId}.{filename}";
    }
    
    // Unused atm, maybe usefull later
    public async static Task<string> GetPipelineImageUrl(byte[] image, string filename)
    {
        var request = new PutObjectRequest()
        {
            BucketName = PIPELINE_IMAGES,
            Key = $"{filename}",
            InputStream = new MemoryStream(image),
        };
        await s3Client.PutObjectAsync(request);
        return $"https://{PIPELINE_IMAGES}.s3.eu-west-3.amazonaws.com/{filename}";
    }

    public async static Task<string> GetPipelineSoundUrl(byte[] sound, string filename)
    {
        var request = new PutObjectRequest()
        {
            BucketName = PIPELINE_SOUNDS,
            Key = $"{filename}",
            InputStream = new MemoryStream(sound),
        };
        await s3Client.PutObjectAsync(request);
        return $"https://{PIPELINE_SOUNDS}.s3.eu-west-3.amazonaws.com/{filename}";
    }

    public async static Task<string> GetPostScriptUrl(string script, Guid postId)
    {
        var request = new PutObjectRequest()
        {
            BucketName = POST_SCRIPTS,
            Key = $"{postId}",
            InputStream = new MemoryStream(Encoding.UTF8.GetBytes(script)),
        };
        await s3Client.PutObjectAsync(request);
        return $"https://{POST_SCRIPTS}.s3.eu-west-3.amazonaws.com/{postId}";
    }

    public async static Task<string> GetConvertedSoundUrl(byte[] sound, string filename)
    {
        var request = new PutObjectRequest()
        {
            BucketName = CONVERTED_SOUNDS,
            Key = $"{filename}",
            InputStream = new MemoryStream(sound),
        };
        await s3Client.PutObjectAsync(request);
        return $"https://{CONVERTED_SOUNDS}.s3.eu-west-3.amazonaws.com/{filename}";
    }
}
