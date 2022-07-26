namespace Api.Models.Entities;

[Table("Commentaries")]
public class Commentary
{
    public Guid Id { get; set; }

    [Required]
    public string Content { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }

    public virtual Account Owner { get; set; }

    public virtual Post Post { get; set; }

    public virtual ICollection<Account> Likes { get; set; }
    public int LikesCount { get; set; }
}
