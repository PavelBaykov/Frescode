using System.Linq;
using System.Web.Mvc;
using Frescode.DAL;

namespace Frescode.Controllers
{
    public class InspectionDrawingController : Controller
    {
        private readonly RootContext _rootContext;

        public InspectionDrawingController(RootContext rootContext)
        {
            _rootContext = rootContext;
        }

        public ActionResult GetInspectionDrawing(int inspectionDrawingId)
        {
            var inspectionDrawing = _rootContext.InspectionDrawingDatas.SingleOrDefault(x => x.Id == inspectionDrawingId);
            return File(inspectionDrawing.Data, "image/png");
        }
    }
}