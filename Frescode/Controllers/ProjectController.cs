using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DALLib;
using DALLib.Entities;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Frescode.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        public ProjectController(IMediator mediator, RootContext rootContext)
            : base(mediator, rootContext)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetBreadcrumbText(int projectId)
        {
            var project = await Context.Projects
                .SingleOrDefaultAsync(x => x.Id == projectId);
            return Json(new { Text = project?.Name }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProjectsList()
        {
            var user = Context.Users.Include(x => x.Projects).SingleOrDefault(x => x.UserName == User.Identity.Name);

            var viewModel = new ProjectsListViewModel();
            foreach (var project in user.Projects)
            {
                var status = "Undefined";
                switch (project.Status)
                {
                    case ProjectStatus.Done:
                        status = "Done";
                        break;
                    case ProjectStatus.InProgress:
                        status = "In Progress";
                        break;
                    case ProjectStatus.Reported:
                        status = "Reported";
                        break;
                }
                var projectViewModel = new ProjectViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    ChangedBy = $"{project.ChangedBy?.FirstName} {project.ChangedBy?.LastName}",
                    DateOfLastChange = project.DateOfLastChange.ToString("MM/dd/yy H:mm:ss"),
                    Status = status
                };
                viewModel.ProjectsList.Add(projectViewModel);
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProjectsList()
        {
            ViewBag.UserId = User.Identity.Name;
            return View();
        }
    }

    public class ProjectsListViewModel
    {
        public ProjectsListViewModel()
        {
            ProjectsList = new List<ProjectViewModel>();
        }
        public List<ProjectViewModel> ProjectsList { get; } 
    }

    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChangedBy { get; set; }
        public string DateOfLastChange { get; set; }
        public string Status { get; set; }
    }
}