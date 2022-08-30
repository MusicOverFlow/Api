using Api.Handlers.Kernel;

namespace Api.Handlers
{
    public class TestHandler : Command<object?, int>
    {
        public TestHandler()
        {

        }

        public object? Handle(int message)
        {

            return null;
        }
    }
}