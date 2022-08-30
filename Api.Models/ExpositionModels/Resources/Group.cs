using System.ComponentModel.DataAnnotations;

namespace Api.Models.ExpositionModels.Resources;

public class GroupResource
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PicUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public AccountResource Owner { get; set; }
}

public class GroupResource_WithMembers : GroupResource
{
    public ICollection<AccountResource> Members { get; set; }
}

public class GroupResource_WithPosts : GroupResource
{
    public ICollection<PostResource> Posts { get; set; }
}

public class GroupResource_WithMembers_AndPosts : GroupResource
{
    public ICollection<AccountResource> Members { get; set; }
    public ICollection<PostResource> Posts { get; set; }
}
