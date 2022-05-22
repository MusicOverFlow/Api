using System.ComponentModel.DataAnnotations;

namespace Api.ExpositionModels;

/**
 * Resource classes
 */
public class AccountResource
{
    public string MailAddress { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountResource_WithPosts : AccountResource
{
    public IEnumerable<PostResource> Posts { get; set; }
}

public class AccountResource_WithGroups : AccountResource
{
    public IEnumerable<GroupResource> Groups { get; set; }
}

public class AccountResource_WithPosts_AndGroups : AccountResource
{
    public IEnumerable<PostResource> Posts { get; set; }
    public IEnumerable<GroupResource> Groups { get; set; }
}

/**
 * Request classes
 */
public class CreateAccount
{
    [Required] public string MailAddress { get; set; }
    [Required] public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}

public class ReadByNames
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}

public class Authentication
{
    [Required] public string MailAddress { get; set; }
    [Required] public string Password { get; set; }
}
