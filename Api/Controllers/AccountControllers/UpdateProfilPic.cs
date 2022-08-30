﻿using Api.Models.ExpositionModels.Resources;
using System.Security.Claims;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPut("profilpic"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<AccountResource>> UpdateProfilPic()
    {
        /*
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(mailAddress));

        if (account == null)
        {
            return NotFound(ExceptionHandler.Get(ErrorType.AccountNotFound));
        }

        if (!Request.Form.Files.Any())
        {
            return BadRequest();
        }

        IFormFile file = Request.Form.Files[0];
        if (!DataValidator.IsImageFormatSupported(file.FileName))
        {
            return BadRequest(ExceptionHandler.Get(ErrorType.WrongFormatFile));
        }
        
        if (file != null && file.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                byte[] fileBytes = ms.ToArray();
                
                account.PicUrl = this.blob.GetProfilPicUrl(fileBytes, account.MailAddress).Result;
            }
        }
        
        await this.context.SaveChangesAsync();

        return Ok(this.mapper.Account_ToResource(account));
        */
        return Ok();
    }
}
