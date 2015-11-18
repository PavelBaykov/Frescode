using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Frescode.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/User/" + User.Identity.GetUserName() + "/");
        }
    }
}