using Api.Models.ExpositionModels.Resources;
using System.Security.Claims;

namespace Api.Controllers.GroupControllers;

public partial class GroupController
{
    [HttpPost, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<GroupResource>> Create(
        [FromForm] string name,
        [FromForm] string description,
        [FromForm] byte[] groupPic)
    {
        /*
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));
        
        if (account == null)
        {
            return NotFound(this.exceptionHandler.GetError(ErrorType.AccountNotFound));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.GroupeMissingName));
        }

        byte[] fileBytes = null;
        if (Request != null)
        {
            IFormFile file = Request.Form.Files.GetFile(nameof(groupPic));
            if (file != null && file.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }
        }
        
        Guid id = Guid.NewGuid();
        Group group = new Group()
        {
            Id = id,
            Name = name,
            Description = description,
            PicUrl = this.blob.GetGroupPicUrl(fileBytes, id).Result,
            CreatedAt = DateTime.Now,
            
            Owner = account,
        };

        this.context.Groups.Add(group);
        account.Groups.Add(group);

        await this.context.SaveChangesAsync();

        return Created(nameof(Create), this.mapper.Group_ToResource(group));
        */
        return Ok();
    }
}
