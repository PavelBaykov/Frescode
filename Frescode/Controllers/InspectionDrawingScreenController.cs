using DALLib;
using MediatR;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Frescode.Controllers
{
    public class InspectionDrawingScreenController : BaseController
    {
        public InspectionDrawingScreenController(IMediator mediator, RootContext rootContext)
            :base(mediator, rootContext)
        {
        }

        // GET: InspectionDrawingScreen
        public ActionResult InspectionDrawingsDetails(int projectId,int inspectionDrawingId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }
    }
}