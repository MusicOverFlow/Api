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
