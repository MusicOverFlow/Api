namespace Api.Models.ExpositionModels.Resources;

public class CommentaryResource
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public AccountResource Owner { get; set; }

    public ICollection<AccountResource> Likes { get; set; }
    public int LikesCount { get; set; }
}

public class CommentaryResource_WithPost : CommentaryResource
{
    public PostResource Post { get; set; }
}

public class CreateCommentary
{
    [Required] public string Content { get; set; }
    public string Script { get; set; }
    public string ScriptLanguage { get; set; }
}
