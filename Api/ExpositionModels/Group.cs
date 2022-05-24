using System.ComponentModel.DataAnnotations;

namespace Api.ExpositionModels;

/**
 * Resource classes
 */
public class GroupResource
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public AccountResource Owner { get; set; }
}

public class GroupResource_WithMembers : GroupResource
{
    public IEnumerable<AccountResource> Members { get; set; }
}

public class GroupResource_WithPosts : GroupResource
{
    public ICollection<PostResource> Posts { get; set; }
}

public class GroupResource_WithMembers_AndPosts : GroupResource
{
    public IEnumerable<AccountResource> Members { get; set; }
    public ICollection<PostResource> Posts { get; set; }
}

/**
 * Request classes
 */
public class CreateGroup
{
    [Required, MaxLength(25)] public string Name { get; set; }
    [Required, MaxLength(100)] public string Description { get; set; }
}