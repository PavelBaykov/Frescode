using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DALLib;
using MediatR;
using DALLib.Entities;
using Microsoft.AspNet.Identity;
using Frescode.BL.Commands;

namespace Frescode.Controllers
{
    [Authorize]
    public class ProjectChecklistController : BaseController
    {
        public ProjectChecklistController(IMediator mediator, RootContext rootContext)
            : base(mediator, rootContext)
        {
            
        }

        [HttpGet]
        public async Task<ActionResult> GetBreadcrumbText(int checklistId)
        {
            var checklist = await Context.ChecklistsForProject
                .Include(x => x.ChecklistTemplate)
                .SingleOrDefaultAsync(x => x.Id == checklistId);
            return Json(new {Text = checklist?.ChecklistTemplate?.Name}, JsonRequestBehavior.AllowGet);
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
            var checklist = Context.ChecklistsForProject
                .Include(x => x.Items.Select(w => w.ItemTemplate))
                .Include(x => x.Items.Select(w => w.ChangedBy))
                .Include(x => x.ChecklistTemplate)
                .Include(x => x.Project)
                .SingleOrDefault(x => x.Id == checklistId);

            var viewModel = new ChecklistItemsListViewModel();
            //viewModel.ChecklistName = checklist.ChecklistTemplate.Name;
            //viewModel.ChecklistProjectName = checklist.Project.Name;
            //viewModel.ChecklistProjectId = checklist.Project.Id;
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
            await Mediator.PublishAsync(new ProjectChecklistItemChangeStateCommand(checklistItemId, newState));

            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
        }

    }

    public class ChecklistItemsListViewModel
    {
        public ChecklistItemsListViewModel()
        {
            ChecklistItemsList = new List<ChecklistItemViewModel>();
        }

        public List<ChecklistItemViewModel> ChecklistItemsList { get; }
        public string ChecklistName { get; set; }
        public string ChecklistProjectName { get; set; }
        public int ChecklistProjectId { get; set; }
    }

    public class ChecklistItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChangedBy { get; set; }
        public string DateOfLastChange { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
    }
}