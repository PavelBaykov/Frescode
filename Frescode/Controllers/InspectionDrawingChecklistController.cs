using DALLib;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using DALLib.Entities;
using Frescode.BL.Commands;

namespace Frescode.Controllers
{
    [Authorize]
    public class InspectionDrawingChecklistController : BaseController
    {

        public InspectionDrawingChecklistController(IMediator mediator, RootContext rootContext)
                : base(mediator, rootContext)
            {

        }

        [HttpGet]
        public async Task<ActionResult> GetBreadcrumbText(int checklistId)
        {

            var checklist = await Context.ChecklistsForInspectionDrawing
                .Include(x => x.ChecklistTemplate)
                .SingleOrDefaultAsync(x => x.Id == checklistId);
            return Json(new { Text = checklist?.ChecklistTemplate?.Name }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChecklistItemsList(int checklistId)
        {
            ViewBag.ChecklistId = checklistId;
            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }

        [HttpGet]
        public ActionResult GetChecklistItemsList(int checklistId)
        {
            var checklist = Context.ChecklistsForInspectionDrawing
                .Include(x => x.Items.Select(w => w.ItemTemplate))
                .Include(x => x.Items.Select(w => w.ChangedBy))
                .Include(x => x.ChecklistTemplate)
                .Include(x => x.InspectionDrawing)
                .SingleOrDefault(x => x.Id == checklistId);

            var viewModel = new ChecklistItemsListViewModel();
            //viewModel.ChecklistName = checklist.ChecklistTemplate.Name;
            //viewModel.ChecklistProjectName = checklist.InspectionDrawing.Name;
            //viewModel.ChecklistProjectId = checklist.InspectionDrawing.Id;
            foreach (var item in checklist.Items)
            {
                var checklistItemViewModel = new ChecklistItemViewModel
                {
                    Id = item.Id,
                    Name = item.ItemTemplate.Name,
                    ChangedBy = $"{item.ChangedBy.FirstName} {item.ChangedBy.LastName}",
                    DateOfLastChange = item.DateOfLastChange.ToString("MM/dd/yy"),
                    Status = item.Status == ChecklistItemStatus.Completed,
                    //DefectSpotsCount = item.DefectionSpots.Count(),
                    Description = item.ItemTemplate.Description
                };
                viewModel.ChecklistItemsList.Add(checklistItemViewModel);
            }
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ChecklistItemSet(int checklistItemId, bool newState)
        {
            await Mediator.PublishAsync(new InspectionDrawingChecklistItemChangeStateCommand(checklistItemId, newState));

            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
        }

    }
}
