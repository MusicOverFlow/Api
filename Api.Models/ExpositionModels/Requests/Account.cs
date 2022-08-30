using Microsoft.AspNetCore.Http;

namespace Api.Models.ExpositionModels.Requests;

public class CreateAccountRequest
{
    public string MailAddress { get; set; }
    public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Pseudonym { get; set; }
    public IFormFile ProfilPic { get; set; }
}

public class FollowRequest
{
    public string CallerMail { get; set; }
    public string TargetMail { get; set; }
}

public class LikeDislikeRequest
{
    public string CallerMail { get; set; }
    public Guid? PostId { get; set; }
}

public class ReadByNamesRequest
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}

public class AuthenticationRequest
{
    [Required] public string MailAddress { get; set; }
    [Required] public string Password { get; set; }
}

public class UpdateProfilRequest
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Pseudonym { get; set; }
}

public class UpdateProfilPicRequest
{
    public byte[] ProfilPic { get; set; }
}
