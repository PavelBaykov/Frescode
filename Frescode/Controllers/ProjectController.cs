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
        public ActionResult GetProjectsList()
        {
            var userId = User.Identity.GetUserId();
            var user = Context.Users.Include(x => x.Projects).SingleOrDefault(x => x.Id == userId);

            var viewModel = new ProjectsListViewModel();
            foreach (var project in user.Projects)
            {
                var projectViewModel = new ProjectViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    ChangedBy = $"{project.ChangedBy?.FirstName} {project.ChangedBy?.LastName}",
                    DateOfLastChange = project.DateOfLastChange.ToString("MM/dd/yy"),
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
        
    }
}