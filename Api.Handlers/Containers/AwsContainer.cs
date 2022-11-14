using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Handlers.Containers;

public class AwsContainer : ContainerNames, IContainer
{
    private readonly IAmazonS3 s3Client;

    public AwsContainer(IConfiguration configuration)
    {
        this.s3Client = new AmazonS3Client(
                awsAccessKeyId: configuration.GetSection("AWSClientCredentials:awsAccessKeyId").Value,
                awsSecretAccessKey: configuration.GetSection("AWSClientCredentials:awsSecretAccessKey").Value,
                region: RegionEndpoint.EUWest3);
    }

    public async Task<string> GetProfilPicUrl(byte[] profilPic, string mailAddress)
    {
        if (profilPic == null || profilPic.Length == 0)
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

    public async Task<string> GetGroupPicUrl(byte[] groupPic, Guid groupId)
    {
        if (groupPic == null || groupPic.Length == 0)
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
        return $"https://{GROUP_PICS}.s3.eu-west-3.amazonaws.com/{groupId}.png";
    }

    public async Task<string> GetMusicUrl(byte[] sound, Guid postId, string filename)
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

    public async Task<string> GetPipelineSoundUrl(byte[] sound, string filename)
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

    public async Task<string> GetPostScriptUrl(string script, Guid postId)
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

    public async Task<string> GetScriptContent(Guid scriptId)
    {
        var request = new GetObjectRequest()
        {
            BucketName = POST_SCRIPTS,
            Key = $"{scriptId}",
        };
        GetObjectResponse response = await s3Client.GetObjectAsync(request);
        using (StreamReader reader = new StreamReader(response.ResponseStream))
        {
            return await reader.ReadToEndAsync();
        }
    }

    public async Task<string> GetConvertedSoundUrl(byte[] sound, string filename)
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

    // Unused atm, maybe usefull later
    public async Task<string> GetPipelineImageUrl(byte[] image, string filename)
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

    public async Task DeletePostScript(Guid postId)
    {
        await s3Client.DeleteObjectAsync(POST_SCRIPTS, postId.ToString());
    }

    public async Task DeletePostSound(string file)
    {
        await s3Client.DeleteObjectAsync(POST_SOUNDS, file);
    }

    public async Task DeleteAccountPic(string mailAddress)
    {
        await s3Client.DeleteObjectAsync(PROFIL_PICS, $"{mailAddress}.png");
    }

    public async Task DeleteGroupPic(Guid groupId)
    {
        await s3Client.DeleteObjectAsync(GROUP_PICS, $"{groupId}.png");
    }
}
