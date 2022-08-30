namespace Api.Handlers.Kernel;

internal interface Handler<R, P>
{
    public R Handle(P message);
}
