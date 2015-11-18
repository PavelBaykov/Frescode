using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frescode.Auth;
using Frescode.DAL;
using MediatR;

namespace Frescode.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IAuthentication authentication, IMediator mediator, RootContext rootContext)
            :base(authentication, mediator, rootContext)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetBreadcrumbText(int userId)
        {
            var user = await Context.Users
                .SingleOrDefaultAsync(x => x.Id == userId);
            return Json(new { Text = $"{user.FirstName} {user.LastName}"}, JsonRequestBehavior.AllowGet);
        }
    }
}