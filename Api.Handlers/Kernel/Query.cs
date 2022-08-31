namespace Api.Handlers.Kernel;

internal interface Query<R, P> : Handler
{
    public R Handle(P message);
}

