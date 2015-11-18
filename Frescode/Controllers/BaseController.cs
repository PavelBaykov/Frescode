using System.Web.Mvc;
using Frescode.Auth;
using Frescode.DAL;
using Frescode.DAL.Entities;
using MediatR;

namespace Frescode.Controllers
{
    public abstract class BaseController : Controller
    {
        public IMediator Mediator { get; }
        public RootContext Context { get; }

        public IAuthentication Auth { get; }
        public User CurrentUser => ((UserIndentity)Auth.CurrentUser.Identity).User;

        protected BaseController(IAuthentication auth, IMediator mediator, RootContext rootContext)
        {
            Mediator = mediator;
            Context = rootContext;
            Auth = auth;
        }
    }
}