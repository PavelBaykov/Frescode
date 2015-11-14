using System.Linq;
using System.Web.Helpers;
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

        public ActionResult GetPictureThumbnail(int pictureId)
        {
            var picture = _rootContext.PicturesData.SingleOrDefault(x => x.Id == pictureId);
            var thumbnail = new WebImage(picture.Data).Resize(150, 100);

            return File(thumbnail.GetBytes(), "image/png");
        }

        public ActionResult DeletePicture(int pictureId)
        {
            var picture = _rootContext.Pictures.SingleOrDefault(x => x.Id == pictureId);
            _rootContext.Pictures.Remove(picture);
            _rootContext.SaveChanges();

            return Json(new
            {
                files = new { picture.Name }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}