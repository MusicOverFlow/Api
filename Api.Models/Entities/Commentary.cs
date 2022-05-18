using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Entities;

[Table("Commentaries")]
public class Commentary
{
    public Guid Id { get; set; }

    [Required]
    public string Content { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }

    public Account Account { get; set; }
    public Post Post { get; set; }
}

public class CommentaryResource
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public AccountResource Account { get; set; }
    public PostResource Post { get; set; }
}

public class CreateCommentary
{
    [Required] public string Content { get; set; }
    [Required] public Guid PostId { get; set; }
}
