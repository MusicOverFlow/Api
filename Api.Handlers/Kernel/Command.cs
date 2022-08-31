namespace Api.Handlers.Kernel;

internal interface Command<R, P> : Handler
{
    public R Handle(P message);
}
