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
    [Required]
    public string Firstname { get; set; }
    [Required]
    public string Lastname { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }

    public ICollection<Post> Posts { get; set; }
    public ICollection<Commentary> Commentaries { get; set; }

    public ICollection<Group> Groups { get; set; }

    public ICollection<Post> LikedPosts { get; set; }
    public ICollection<Commentary> LikedCommentaries { get; set; }
}
