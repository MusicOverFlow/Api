namespace Api.Models.Entities;

[Table("Posts")]
public class Post
{
    public Guid Id { get; set; }

    [Required] public string Title { get; set; }
    [Required] public string Content { get; set; }
    public int LikesCount { get; set; }
    public string MusicUrl { get; set; }
    public string Script { get; set; }
    public string ScriptLanguage { get; set; }

    public virtual Account Owner { get; set; }
    public virtual Group Group { get; set; }
    public virtual ICollection<Commentary> Commentaries { get; set; } = new List<Commentary>();
    public virtual ICollection<Account> Likes { get; set; } = new List<Account>();

    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now;
}
