using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Frescode.DAL;
using MediatR;

namespace Frescode.Controllers
{
    public class PictureController : BaseController
    {

        public PictureController(IMediator mediator, RootContext rootContext)
            :base(mediator, rootContext)
        {
        }

        public ActionResult GetPicture(int pictureId)
        {
            var picture = Context.PicturesData.SingleOrDefault(x => x.Id == pictureId);
            return File(picture.Data, "image/png");
        }

        public ActionResult GetPictureThumbnail(int pictureId)
        {
            var picture = Context.PicturesData.SingleOrDefault(x => x.Id == pictureId);
            var thumbnail = new WebImage(picture.Data).Resize(150, 100);

            return File(thumbnail.GetBytes(), "image/png");
        }

        public ActionResult DeletePicture(int pictureId)
        {
            var picture = Context.Pictures.SingleOrDefault(x => x.Id == pictureId);
            Context.Pictures.Remove(picture);
            Context.SaveChanges();

            return Json(new
            {
                files = new { picture.Name }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}