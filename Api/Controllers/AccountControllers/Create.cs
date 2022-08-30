using Api.Handlers.Commands;
using Api.Handlers.Dtos;
using Api.Handlers.Kernel;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromForm] string mailAddress,
        [FromForm] string password,
        [FromForm] string firstname,
        [FromForm] string lastname,
        [FromForm] string pseudonym,
        [FromForm] byte[] profilPic)
    {
        try
        {
            Account account = await this.handlers.Get<CreateAccountCommand>().Handle(new CreateAccountDto()
            {
                MailAddress = mailAddress != null ? mailAddress.Trim() : string.Empty,
                Password = password,
                Firstname = firstname ?? "Unknown",
                Lastname = lastname ?? "Unknown",
                Pseudonym = pseudonym ?? "Anonymous",
                ProfilPic = Request.Form.Files.GetFile(nameof(profilPic)),
            });

            return Ok(Mapper.Account_ToResource(account));
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}
