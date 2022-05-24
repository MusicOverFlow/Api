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

    public Account Owner { get; set; }

    public ICollection<Commentary> Commentaries { get; set; }

    public Group Group { get; set; }

    public ICollection<Account> Likes { get; set; }
    public int LikesCount { get; set; }
}
