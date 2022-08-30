namespace Api.Handlers;

public abstract class HandlerBase
{
    protected readonly ModelsContext context;

    protected HandlerBase(ModelsContext context)
    {
        this.context = context;
    }
}
