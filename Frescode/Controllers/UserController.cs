using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using DALLib;
using MediatR;

namespace Frescode.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator, RootContext rootContext)
            :base(mediator, rootContext)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetBreadcrumbText(string userId)
        {
            var user = await Context.Users
                .SingleOrDefaultAsync(x => x.UserName == userId);
            return Json(new { Text = $"{user.FirstName} {user.LastName}"}, JsonRequestBehavior.AllowGet);
        }

    }
}