using Api.Handlers.Queries.AuthenticationQueries;

namespace Api.Controllers.AuthenticationControllers;

public partial class AuthenticationController
{
    [HttpPost]
    public async Task<ActionResult> Authenticate(AuthenticationRequest request)
    {
        try
        {
            AuthenticationQuery handler = this.handlers.Get<AuthenticationQuery>();
            handler.Configuration = this.configuration;
            string jwt = await handler.Handle(new AuthenticationDto()
            {
                MailAddress = request.MailAddress,
                Password = request.Password,
            });

            return Ok(new { jwt });
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}