using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frescode.DAL;

namespace Frescode.Controllers
{
    public class UserController : Controller
    {
        private readonly RootContext _rootContext;

        public UserController(RootContext rootContext)
        {
            _rootContext = rootContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetBreadcrumbText(int userId)
        {
            var user = await _rootContext.Users
                .SingleOrDefaultAsync(x => x.Id == userId);
            return Json(new { Text = $"{user.FirstName} {user.LastName}"}, JsonRequestBehavior.AllowGet);
        }
    }
}