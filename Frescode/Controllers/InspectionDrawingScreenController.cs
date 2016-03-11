using DALLib;
using DALLib.Entities;
using MediatR;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult InspectionDrawingsDetails(int projectId,int drawingId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.DrawingId = drawingId;
            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }

        public async Task<ActionResult> GetBreadcrumbText(int drawingId)
        {
            var inspectionDrawing = await Context.InspectionDrawings
                .Include(drawing => drawing.InspectionDrawingTemplate)
                .SingleOrDefaultAsync(drawing => drawing.Id == drawingId);

            return Json(new { Text = inspectionDrawing.InspectionDrawingTemplate?.Name} ,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChecklistsList(int drawingId)
        {
            var inspectionDrawing = Context.InspectionDrawings
                .Include(drawing => drawing.Checklists)
                .Include(drawing => drawing.Checklists.Select(ch => ch.ChecklistTemplate))
                .Include(drawing => drawing.Checklists.Select(ch => ch.Items))
                .Include(drawing => drawing.Checklists.Select(ch => ch.ChangedBy))
                .SingleOrDefault(drawing => drawing.Id == drawingId);
            var viewModel = new ChecklistsListViewModel();
            foreach (var checklist in inspectionDrawing.Checklists)
            {
                var checklistViewModel = new ChecklistViewModel
                {
                    Id = checklist.Id,
                    Name = checklist.ChecklistTemplate.Name,
                    ChangedBy = $"{checklist.ChangedBy.FirstName} {checklist.ChangedBy.LastName}",
                    DateOfLastChange = checklist.DateOfLastChange.ToString("MM/dd/yy"),
                    Status = $"{checklist.Items.Count(x => x.Status == ChecklistItemStatus.Completed)}/{checklist.Items.Count()}"
                };
                viewModel.ChecklistsList.Add(checklistViewModel);
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
    }


}