using System.ComponentModel.DataAnnotations;

namespace Api.ExpositionModels;

/**
 * Resource classes
 */
public class CommentaryResource
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public AccountResource Account { get; set; }
}

public class CommentaryResource_WithPost : CommentaryResource
{
    public PostResource Post { get; set; }
}

/**
 * Request classes
 */
public class CreateCommentary
{
    [Required] public string Content { get; set; }
}
