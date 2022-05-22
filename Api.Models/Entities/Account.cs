using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities;

[Table("Accounts")]
public class Account
{
    public Guid Id { get; set; }

    [Required]
    public string MailAddress { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
    [Required]
    public byte[] PasswordSalt { get; set; }
    [Required]
    public string Role { get; set; }
    [Required, MaxLength(20)]
    public string Firstname { get; set; }
    [Required, MaxLength(20)]
    public string Lastname { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }

    public ICollection<Post> Posts { get; set; }
    public ICollection<Commentary> Commentaries { get; set; }
}

public class AccountResource
{
    public string MailAddress { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<PostResource> Posts { get; set; }
    public ICollection<PostResource> Commentaries { get; set; }
}

public class CreateAccount
{
    [Required] public string MailAddress { get; set; }
    [Required] public string Password { get; set; }
    public string Firstname { get; set; } = "Unknown";
    public string Lastname { get; set; } = "Unknown";
}

public class ReadByNames
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}

public class Authentication
{
    [Required] public string MailAddress { get; set; }
    [Required] public string Password { get; set; }
}
