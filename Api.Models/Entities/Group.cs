namespace Api.Models.Entities;

[Table("Groups")]
public class Group
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }
    [Required] public string PicUrl { get; set; }
    public string Description { get; set; }

    public Account Owner { get; set; }
    public ICollection<Account> Members { get; set; }
    public ICollection<Post> Posts { get; set; }

    [Required] public DateTime CreatedAt { get; set; }
}
