using Microsoft.AspNetCore.Http;

namespace Api.Handlers.Dtos;

public class CreateAccountDto
{
    public string MailAddress { get; set; }
    public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Pseudonym { get; set; }
    public IFormFile ProfilPic { get; set; }
}

public class FollowDto
{
    public string CallerMail { get; set; }
    public string TargetMail { get; set; }
}

public class LikeDislikeDto
{
    public string CallerMail { get; set; }
    public Guid? PostId { get; set; }
}

public class ReadByNamesDto
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}

public class UpdateMailDto
{
    public string MailAddress { get; set; }
    public string NewMailAddress { get; set; }
}

public class UpdatePasswordDto
{
    public string MailAddress { get; set; }
    public string NewPassword { get; set; }
}

public class UpdateProfilDto
{
    public string MailAddress { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Pseudonym { get; set; }
}

public class UpdateProfilPicDto
{
    public string MailAddress { get; set; }
    public IFormFile ProfilPic { get; set; }
}

public class UpdateAccountRoleDto
{
    public string MailAddress { get; set; }
    public string Role { get; set; }
}