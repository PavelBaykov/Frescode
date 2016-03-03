using DALLib;
using MediatR;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DALLib.Entities;

namespace Frescode.Controllers
{
    [Authorize]
    public class ProjectScreenController : BaseController
    {

        public ProjectScreenController(IMediator mediator, RootContext rootContext)
            : base(mediator, rootContext)
        {

        }

        // GET: ProjectScreen
        public ActionResult ProjectScreenList(int projectId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.UserId = User.Identity.GetUserId(); 
            return View();
        }

        [HttpGet]
        public ActionResult GetChecklistsList(int projectId)
        {
            var project = Context.Projects
                .Include(x => x.Checklists.Select(w => w.ChecklistTemplate))
                .Include(x => x.Checklists.Select(w => w.ChangedBy))
                .Include(x => x.Checklists.Select(w => w.Items))
                .SingleOrDefault(x => x.Id == projectId);
            var viewModel = new ChecklistsListViewModel();
            viewModel.ProjectName = project.Name;
            foreach (var checklist in project.Checklists)
            {
                var checklistViewModel = new ChecklistViewModel
                {
                    Id = checklist.Id,
                    Name = checklist.ChecklistTemplate.Name,
                    ChangedBy = $"{checklist.ChangedBy.FirstName} {checklist.ChangedBy.LastName}",
                    DateOfLastChange = checklist.DateOfLastChange.ToString("MM/dd/yy H:mm:ss"),
                    Status = $"{checklist.Items.Count(x => x.Status == ChecklistItemStatus.Completed)}/{checklist.Items.Count()}"
                };
                viewModel.ChecklistsList.Add(checklistViewModel);
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
    }

    public class ChecklistsListViewModel
    {
        public ChecklistsListViewModel()
        {
            ChecklistsList = new List<ChecklistViewModel>();
        }
        public List<ChecklistViewModel> ChecklistsList { get; }
        public string ProjectName { get; set; }
    }

    public class ChecklistViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChangedBy { get; set; }
        public string DateOfLastChange { get; set; }
        public string Status { get; set; }
    }
}