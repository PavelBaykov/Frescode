using System.Linq;
using System.Web.Mvc;
using Frescode.Auth;
using Frescode.DAL;
using MediatR;

namespace Frescode.Controllers
{
    public class InspectionDrawingController : BaseController
    {
        public InspectionDrawingController(IAuthentication authentication, IMediator mediator, RootContext rootContext)
            : base(authentication, mediator, rootContext)
        {
        }

        public ActionResult GetInspectionDrawing(int inspectionDrawingId)
        {
            var inspectionDrawing = Context.InspectionDrawingDatas.SingleOrDefault(x => x.Id == inspectionDrawingId);
            return File(inspectionDrawing.Data, "image/png");
        }
    }
}