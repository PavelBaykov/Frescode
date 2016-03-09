﻿using System.Linq;
using System.Web.Mvc;
using DALLib;
using MediatR;

namespace Frescode.Controllers
{
    [Authorize]
    public class InspectionDrawingController : BaseController
    {
        public InspectionDrawingController(IMediator mediator, RootContext rootContext)
            : base(mediator, rootContext)
        {
        }

        //public ActionResult GetInspectionDrawing(int inspectionDrawingId)
        //{
        //    var inspectionDrawing = Context.InspectionDrawingDatas.SingleOrDefault(x => x.Id == inspectionDrawingId);
        //    return File(inspectionDrawing.Data, "image/png");
        //}
    }
}