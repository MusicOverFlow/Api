using System.ComponentModel.DataAnnotations;

namespace Api.ExpositionModels;

/**
 * Resource classes
 */
public class PostResource
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public AccountResource Owner { get; set; }
    
    public ICollection<CommentaryResource> Commentaries { get; set; }
    
    public GroupResource Group { get; set; }
    
    public ICollection<AccountResource> Likes { get; set; }
    public int LikesCount { get; set; }
}

/**
 * Request classes
 */
public class CreatePost
{
    [Required, MaxLength(50)]  public string Title { get; set; }
    [Required, MaxLength(250)] public string Content { get; set; }
}
