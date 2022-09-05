namespace Api.Models.Entities;

[Table("Posts")]
public class Post
{
    public Guid Id { get; set; }

    [Required] public string Title { get; set; }
    [Required] public string Content { get; set; }
    public int LikesCount { get; set; }
    public string MusicUrl { get; set; }
    public string ScriptUrl { get; set; }
    public string ScriptLanguage { get; set; }

    public Account Owner { get; set; }
    public Group Group { get; set; }
    public ICollection<Commentary> Commentaries { get; set; }
    public ICollection<Account> Likes { get; set; }

    [Required] public DateTime CreatedAt { get; set; }
}
