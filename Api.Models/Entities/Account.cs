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
    public string Pseudonym { get; set; }
    [Required]
    public string PicUrl { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Post> OwnedPosts { get; set; }
    public virtual ICollection<Commentary> OwnedCommentaries { get; set; }

    public virtual ICollection<Group> Groups { get; set; }

    public virtual ICollection<Post> LikedPosts { get; set; }
    public virtual ICollection<Commentary> LikedCommentaries { get; set; }
    
    public virtual ICollection<Account> Follows { get; set; }
}
