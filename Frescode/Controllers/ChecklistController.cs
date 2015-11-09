using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Frescode.DAL;
using Frescode.DAL.Entities;
using MediatR;

namespace Frescode.Controllers
{
    public class ChecklistController : Controller
    {
        private readonly IMediator _mediator;
        private readonly RootContext _rootContext;

        public ChecklistController(IMediator mediator, RootContext rootContext)
        {
            _mediator = mediator;
            _rootContext = rootContext;
        }

        // GET: Checklist
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChecklistsList(int userId, int projectId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.UserId = userId;
            return View();
        }

        [HttpGet]
        public ActionResult GetChecklistsList(int projectId)
        {
            var project = _rootContext.Projects
                .Include(x => x.Checklists.Select(w => w.ChecklistTemplate))
                .Include(x => x.Checklists.Select(w => w.ChangedBy))
                .Include(x => x.Checklists.Select(w => w.Items))
                .SingleOrDefault(x => x.Id == projectId);
            var viewModel = new ChecklistsListViewModel();
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