using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers.Kernel;

public class HandlerException : Exception
{
    public readonly ObjectResult Content;

    public HandlerException(ErrorType errorType)
    {
        this.Content = new ObjectResult(null);
        this.Content.StatusCode = ExceptionHandler.GetCode(errorType);
        this.Content.Value = ExceptionHandler.Get(errorType);
    }
}
