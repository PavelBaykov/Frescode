﻿using DALLib;
using MediatR;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DALLib.Entities;
using System.Threading.Tasks;

namespace Frescode.Controllers
{
    [Authorize]
    public class ProjectScreenController : BaseController
    {
        public ProjectScreenController(IMediator mediator, RootContext rootContext)
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

        // GET: ProjectScreen
        public ActionResult ProjectScreenList(int projectId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.UserId = User.Identity.GetUserId(); 
            return View();
        }

        [HttpGet]
        public ActionResult GetProjectChecklistsList(int projectId)
        {
            var project = Context.Projects
                .Include(x => x.Checklists.Select(w => w.ChecklistTemplate))
                .Include(x => x.Checklists.Select(w => w.ChangedBy))
                .Include(x => x.Checklists.Select(w => w.Items))
                .SingleOrDefault(x => x.Id == projectId);
            var viewModel = new ProjectChecklistsListViewModel();
            foreach (var checklist in project.Checklists)
            {
                var checklistViewModel = new ProjectChecklistViewModel
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

        [HttpGet]
        public ActionResult GetStructures(int projectId)
        {
            var structuresFromDb = Context.Structures
                .Include(s=> s.Project)
                .Include(s=> s.InspectionDrawing)
                .Where(s => s.Project.Id == projectId);

            var structures = new StructuresListViewModel();

            foreach (var s in structuresFromDb)
            {
                var tmpStruct = new StructureViewModel();
                tmpStruct.Name = s.Name;
                tmpStruct.Path = s.Path;
                tmpStruct.Id = s.Id;
                tmpStruct.InspectionDrawingId = s.InspectionDrawing?.Id;

                structures.Structures.Add(tmpStruct);
            }

            return Json(structures, JsonRequestBehavior.AllowGet);
        }
    }

    public class ProjectChecklistsListViewModel
    {
        public ProjectChecklistsListViewModel()
        {
            ChecklistsList = new List<ProjectChecklistViewModel>();
        }
        public List<ProjectChecklistViewModel> ChecklistsList { get; }
    }

    public class ProjectChecklistViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChangedBy { get; set; }
        public string DateOfLastChange { get; set; }
        public string Status { get; set; }
    }

    public class StructuresListViewModel
    {
        public StructuresListViewModel()
        {
            Structures = new List<StructureViewModel>();
        }
        public List<StructureViewModel> Structures { get; set; }
       
    }

    public class StructureViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int? InspectionDrawingId { get; set; }
    }
}