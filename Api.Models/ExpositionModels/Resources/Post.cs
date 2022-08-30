namespace Api.Models.ExpositionModels.Resources;

public class PostResource
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public int LikesCount { get; set; }
    public string MusicUrl { get; set; }
    public string ScriptUrl { get; set; }
    public string ScriptLanguage { get; set; }

    public AccountResource Owner { get; set; }
    public GroupResource Group { get; set; }
    public ICollection<AccountResource> Likes { get; set; }
    public ICollection<CommentaryResource> Commentaries { get; set; }
}

public class CreatePost
{
    [Required, MaxLength(50)] public string Title { get; set; }
    [Required, MaxLength(250)] public string Content { get; set; }
    public string Script { get; set; }
    public string ScriptLanguage { get; set; }
}
