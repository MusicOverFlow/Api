using Microsoft.AspNetCore.Http;

namespace Api.Handlers.Dtos;

public class CreateGroupDto
{
    public string CreatorMailAddress { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile GroupPic { get; set; }
}

public class JoinGroupDto
{
    public string MailAddress { get; set; }
    public Guid GroupId { get; set; }
}

public class KickMemberDto
{
    public string CallerMailAddress { get; set; }
    public string MemberMailAddress { get; set; }
    public Guid GroupId { get; set; }
}

public class LeaveGroupDto
{
    public string MailAddress { get; set; }
    public Guid GroupId { get; set; }
}

public class ReadGroupPostsDto
{
    public string MailAddress { get; set; }
    public Guid? GroupId { get; set; }
}
