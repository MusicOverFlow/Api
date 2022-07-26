using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpGet("groupPosts")]
    public async Task<ActionResult<List<PostResource>>> ReadAccountGroupPosts()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        return Ok(account.Groups
            .SelectMany(g => g.Posts)
            .Select(p => this.mapper.Post_ToResource(p)));
    }
}
