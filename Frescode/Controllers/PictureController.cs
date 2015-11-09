using System.Linq;
using System.Web.Mvc;
using Frescode.DAL;

namespace Frescode.Controllers
{
    public class PictureController : Controller
    {
        private readonly RootContext _rootContext;

        public PictureController(RootContext rootContext)
        {
            _rootContext = rootContext;
        }

        public ActionResult GetPicture(int pictureId)
        {
            var picture = _rootContext.PicturesData.SingleOrDefault(x => x.Id == pictureId);
            return File(picture.Data, "image/png");
        }
    }
}