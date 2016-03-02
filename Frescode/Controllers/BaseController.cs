using System.Web.Mvc;
using DALLib;
using MediatR;

namespace Frescode.Controllers
{
    public abstract class BaseController : Controller
    {
        public IMediator Mediator { get; }
        public RootContext Context { get; }

        protected BaseController(IMediator mediator, RootContext rootContext)
        {
            Mediator = mediator;
            Context = rootContext;
        }
    }
}