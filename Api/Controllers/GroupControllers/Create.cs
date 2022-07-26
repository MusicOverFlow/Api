using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost]
    public async Task<ActionResult<GroupResource>> Create(CreateGroup request)
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));
        
        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.GroupeMissingName));
        }

        string picUrl = string.Empty;
        IFormFile file = Request.Form.Files.Any() ? Request.Form.Files[0] : new FormFileCollection()[0];

        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            byte[] fileBytes = ms.ToArray();

            picUrl = this.GetGroupPicUrl(Request.Form.Files.Any() ? fileBytes : null).Result;
        }

        Group group = new Group()
        {
            Name = request.Name,
            Description = request.Description,
            PicUrl = picUrl,
            CreatedAt = DateTime.Now,
            
            Owner = account,
        };

        this.context.Groups.Add(group);
        account.Groups.Add(group);

        await this.context.SaveChangesAsync();

        return Created(nameof(Create), this.mapper.Group_ToResource(group));
    }
}
