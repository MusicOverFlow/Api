namespace Api.Models.Entities;

[Table("Commentaries")]
public class Commentary
{
    public Guid Id { get; set; }

    [Required] public string Content { get; set; }

    public string Script { get; set; }
    public string ScriptLanguage { get; set; }
    public int LikesCount { get; set; }

    public virtual Account Owner { get; set; }
    public virtual Post Post { get; set; }
    public virtual ICollection<Account> Likes { get; set; } = new List<Account>();

    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now;
}
