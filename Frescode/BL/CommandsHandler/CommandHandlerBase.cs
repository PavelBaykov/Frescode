using Frescode.DAL;

namespace Frescode.BL.CommandsHandler
{
    public abstract class CommandHandlerBase
    {
        private readonly RootContext _rootContext;

        protected CommandHandlerBase(RootContext rootContext)
        {
            _rootContext = rootContext;
        }
    }
}
