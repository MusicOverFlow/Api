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
