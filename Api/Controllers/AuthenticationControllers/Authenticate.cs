using Api.Handlers.Kernel;
using Api.Handlers.Queries;
using Api.Models.ExpositionModels.Requests;

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
            string jwt = await handler.Handle(request);

            return Ok(new { jwt });
        }
        catch (HandlerException exception)
        {
            return exception.Content;
        }
    }
}