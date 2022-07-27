namespace Api.Models.Entities;

[Table("Groups")]
public class Group
{
    public Guid Id { get; set; }

    [Required, MaxLength(25)]
    public string Name { get; set; }
    [Required, MaxLength(100)]
    public string Description { get; set; }
    [Required]
    public string PicUrl { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }

    public virtual Account Owner { get; set; }
    public virtual ICollection<Account> Members { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
}
