namespace Api.Models.Entities;

[Table("Posts")]
public class Post
{
    public Guid Id { get; set; }

    [Required, MaxLength(50)]
    public string Title { get; set; }
    [Required, MaxLength(250)]
    public string Content { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }

    public virtual Account Owner { get; set; }

    public virtual ICollection<Commentary> Commentaries { get; set; }

    public virtual Group Group { get; set; }

    public virtual ICollection<Account> Likes { get; set; }
    public int LikesCount { get; set; }
}
