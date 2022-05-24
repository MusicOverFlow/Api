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
    public DateTime CreatedAt { get; set; }

    public Account Owner { get; set; }
    public ICollection<Account> Members { get; set; }
    public ICollection<Post> Posts { get; set; }
}
