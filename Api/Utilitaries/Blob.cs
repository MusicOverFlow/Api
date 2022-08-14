using Amazon.S3.Model;
using System.Text;

namespace Api.Utilitaries;

public class Blob
{
    private const string PROFIL_PICS = "profil-pics";
    private const string GROUP_PICS = "group-pics";
    private const string POST_SOUNDS = "post-sounds";
    private const string PIPELINE_IMAGES = "pipeline-images-mo";
    private const string PIPELINE_SOUNDS = "pipeline-sounds";
    private const string POST_SCRIPTS = "post-scripts";
    private const string CONVERTED_SOUNDS = "converted-sounds-mo";

    private readonly IAmazonS3 s3Client;
    
    public Blob(IAmazonS3 s3Client)
    {
        this.s3Client = s3Client;
    }
    
    public async Task<string> GetProfilPicUrl(byte[] profilPic, string mailAddress)
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
        await this.s3Client.PutObjectAsync(request);
        return $"https://{PROFIL_PICS}.s3.eu-west-3.amazonaws.com/{mailAddress}.png";
    }

    public async Task<string> GetGroupPicUrl(byte[] groupPic, Guid groupId)
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
        await this.s3Client.PutObjectAsync(request);
        return $"https://{PROFIL_PICS}.s3.eu-west-3.amazonaws.com/{groupId}.png";
    }

    public async Task<string> GetMusicUrl(byte[] sound, Guid postId, string filename)
    {
        var request = new PutObjectRequest()
        {
            BucketName = POST_SOUNDS,
            Key = $"{postId}.{filename}",
            InputStream = new MemoryStream(sound),
        };
        await this.s3Client.PutObjectAsync(request);
        return $"https://{POST_SOUNDS}.s3.eu-west-3.amazonaws.com/{postId}.{filename}";
    }
    
    public async Task<string> GetPipelineImageUrl(byte[] image, string filename)
    {
        var request = new PutObjectRequest()
        {
            BucketName = PIPELINE_IMAGES,
            Key = $"{filename}",
            InputStream = new MemoryStream(image),
        };
        await this.s3Client.PutObjectAsync(request);
        return $"https://{PIPELINE_IMAGES}.s3.eu-west-3.amazonaws.com/{filename}";
    }

    public async Task<string> GetPipelineSoundUrl(byte[] sound, string filename)
    {
        var request = new PutObjectRequest()
        {
            BucketName = PIPELINE_SOUNDS,
            Key = $"{filename}",
            InputStream = new MemoryStream(sound),
        };
        await this.s3Client.PutObjectAsync(request);
        return $"https://{PIPELINE_SOUNDS}.s3.eu-west-3.amazonaws.com/{filename}";
    }

    public async Task<string> GetPostScriptUrl(string script, Guid postId)
    {
        var request = new PutObjectRequest()
        {
            BucketName = POST_SCRIPTS,
            Key = $"{postId}",
            InputStream = new MemoryStream(Encoding.UTF8.GetBytes(script)),
        };
        await this.s3Client.PutObjectAsync(request);
        return $"https://{POST_SCRIPTS}.s3.eu-west-3.amazonaws.com/{postId}";
    }

    public async Task<string> GetConverterSoundUrl(byte[] sound, string filename)
    {
        var request = new PutObjectRequest()
        {
            BucketName = CONVERTED_SOUNDS,
            Key = $"{filename}",
            InputStream = new MemoryStream(sound),
        };
        await this.s3Client.PutObjectAsync(request);
        return $"https://{CONVERTED_SOUNDS}.s3.eu-west-3.amazonaws.com/{filename}";
    }
}
