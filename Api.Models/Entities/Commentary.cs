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

    public Account Owner { get; set; }
    
    public Post Post { get; set; }

    public ICollection<Account> Likes { get; set; }
    public int LikesCount { get; set; }
}
