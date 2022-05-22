using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public Account Account { get; set; }
    public ICollection<Commentary> Commentaries { get; set; }
    
    public Group Group { get; set; }
}

public class PostResource
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public AccountResource Account { get; set; }
    public ICollection<CommentaryResource> Commentaries { get; set; }
    public GroupResource Group { get; set; }
}

public class CreatePost
{
    [Required, MaxLength(50)]  public string Title { get; set; }
    [Required, MaxLength(250)] public string Content { get; set; }
    public Guid? GroupId { get; set; }
}

// TODO: une route sur l'api pour savoir si un post est liké par un utilisateur ou pas
public class LikeDislike
{
    [Required] public Guid PostId { get; set; }
}
