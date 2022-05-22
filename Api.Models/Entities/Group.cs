using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

public class GroupResource
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public AccountResource Owner { get; set; }
    public ICollection<AccountResource> Members { get; set; }
    public ICollection<PostResource> Posts { get; set; }
}

public class CreateGroup
{
    [Required, MaxLength(25)]  public string Name { get; set; }
    [Required, MaxLength(100)] public string Description { get; set; }
}