namespace Api.Models.Entities;

[Table("Groups")]
public class Group
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }
    [Required] public string PicUrl { get; set; }
    public string Description { get; set; }

    public virtual Account Owner { get; set; }
    public virtual ICollection<Account> Members { get; set; } = new List<Account>();
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now;
}
