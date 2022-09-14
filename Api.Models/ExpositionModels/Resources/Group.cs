namespace Api.Models.ExpositionModels.Resources;

public class GroupResource
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PicUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public AccountResource Owner { get; set; }
    public ICollection<AccountResource> Members { get; set; }
    public ICollection<PostResource> Posts { get; set; }
}
