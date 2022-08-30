using Api.Models.ExpositionModels.Resources;
using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet("groupPosts"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<PostResource>>> ReadAccountGroupPosts()
    {
        /*
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(ExceptionHandler.Get(ErrorType.AccountNotFound));
        }

        List<PostResource> posts = new List<PostResource>();
        account.Groups
            .ToList()
            .ForEach(g => g.Posts
                .ToList()
                .ForEach(p => posts.Add(this.mapper.Post_ToResource(p))));

        return Ok(posts);
        */
        return Ok();
    }
}
