using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frescode.DAL;
using Frescode.DAL.Entities;
using MediatR;

namespace Frescode.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;
        private readonly RootContext _rootContext;

        public ProjectController(IMediator mediator, RootContext rootContext)
        {
            _mediator = mediator;
            _rootContext = rootContext;
        }

        [HttpGet]
        public ActionResult GetProjectsList(int userId)
        {
            var user = _rootContext.Users.Include(x => x.Projects).SingleOrDefault(x => x.Id == userId);

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
                    ChangedBy = $"{project.ChangedBy.FirstName} {project.ChangedBy.LastName}",
                    DateOfLastChange = project.DateOfLastChange.ToString("MM/dd/yy H:mm:ss"),
                    Status = status
                };
                viewModel.ProjectsList.Add(projectViewModel);
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProjectsList(int userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> InitDb()
        {
            _rootContext.Projects.RemoveRange(_rootContext.Projects);
            _rootContext.Users.RemoveRange(_rootContext.Users);
            _rootContext.Customers.RemoveRange(_rootContext.Customers);
            _rootContext.Checklists.RemoveRange(_rootContext.Checklists);
            _rootContext.ChecklistItems.RemoveRange(_rootContext.ChecklistItems);
            _rootContext.ChecklistTemplates.RemoveRange(_rootContext.ChecklistTemplates);
            _rootContext.ChecklistItemTemplates.RemoveRange(_rootContext.ChecklistItemTemplates);
            _rootContext.DefectionSpots.RemoveRange(_rootContext.DefectionSpots);
            _rootContext.InspectionDrawings.RemoveRange(_rootContext.InspectionDrawings);
            _rootContext.InspectionDrawingDatas.RemoveRange(_rootContext.InspectionDrawingDatas);
            _rootContext.Pictures.RemoveRange(_rootContext.Pictures);
            _rootContext.PicturesData.RemoveRange(_rootContext.PicturesData);
            _rootContext.SaveChanges();

            var customer = new Customer
            {
                Projects = new List<Project>(),
                Users = new List<User>()
            };
            
            var user = new User
            {
                FirstName = "Pavel",
                LastName = "Baykov",
                DateCreated = DateTime.UtcNow,
                Username = "pavelbaykov89",
                Password = "123456",
                UserRole = UserRole.Reader,
                Projects = new List<Project>(),
                Customer = customer
            };

            var project = new Project
            {
                Customer = customer,
                DateCreated = DateTime.UtcNow,
                Owner = user,
                CreatedBy = user,
                ChangedBy = user,
                DateOfLastChange = DateTime.UtcNow,
                Name = "My First Project",
                Checklists = new List<Checklist>(),
                Members = new List<User> {user},
                Status = ProjectStatus.Done
            };

            var httpClient = new HttpClient();
            var floorPlanBytes = await httpClient.GetByteArrayAsync(
                "http://www.hollandaleapartments.com/images/floor%20plans/Embassy-floor-plan.png");
            var inspectionDrawingData = new InspectionDrawingData
            {
                Data = floorPlanBytes
            };

            var inspectionDrawing = new InspectionDrawing
            {
                CreatedBy = user,
                DateCreated = DateTime.Now,
                Name = "Inspection Drawing 1",
                Size = 1024,
                InspectionDrawingData = inspectionDrawingData
            };

            var checklistTemplate = new ChecklistTemplate
            {
                Name = "Equipment",
                Items = new List<ChecklistItemTemplate>(),
                Descendants = new List<Checklist>()
            };
            var checklistItemTemplate1 = new ChecklistItemTemplate
            {
                Name = "Item 1",
                Checklist = checklistTemplate,
                Description = "Description of Item 1",
                OrderNumber = 2,
                Descendants = new List<ChecklistItem>(),
                InspectionDrawing = inspectionDrawing
            };
            var checklistItemTemplate2 = new ChecklistItemTemplate
            {
                Name = "Item 2",
                Checklist = checklistTemplate,
                Description = "Description of Item 2",
                OrderNumber = 1,
                Descendants = new List<ChecklistItem>()
            };
            checklistTemplate.Items.Add(checklistItemTemplate1);
            checklistTemplate.Items.Add(checklistItemTemplate2);
            var checklist = new Checklist
            {
                Project = project,
                ChangedBy = user,
                DateCreated = DateTime.UtcNow,
                DateOfLastChange = DateTime.UtcNow,
                CreatedBy = user,
                ChecklistTemplate = checklistTemplate,
                Items = new List<ChecklistItem>()
            };
            checklistTemplate.Descendants.Add(checklist);
            project.Checklists.Add(checklist);
            var checklistItem1 = new ChecklistItem
            {
                ChangedBy = user,
                Checklist = checklist,
                ItemTemplate = checklistItemTemplate1,
                Status = ChecklistItemStatus.NotCompleted,
                DateOfLastChange = DateTime.UtcNow,
                DefectionSpots = new List<DefectionSpot>()
            };
            var checklistItem2 = new ChecklistItem
            {
                ChangedBy = user,
                Checklist = checklist,
                ItemTemplate = checklistItemTemplate2,
                Status = ChecklistItemStatus.Completed,
                DateOfLastChange = DateTime.UtcNow,
                DefectionSpots = new List<DefectionSpot>()
            };
            checklistItemTemplate1.Descendants.Add(checklistItem1);
            checklistItemTemplate2.Descendants.Add(checklistItem2);
            checklist.Items.Add(checklistItem1);
            checklist.Items.Add(checklistItem2);

            var defectSpot1 = new DefectionSpot
            {
                ChecklistItem = checklistItem1,
                DateCreated = DateTime.UtcNow,
                Description = "Description of defect spot",
                X = 100,
                Y = 300,
                OrderNumber = 2,
                AttachedPictures = new List<Picture>()
            };
            var defectSpot2 = new DefectionSpot
            {
                ChecklistItem = checklistItem2,
                DateCreated = DateTime.UtcNow,
                Description = "Description of defect spot",
                X = 100,
                Y = 300,
                OrderNumber = 1,
                AttachedPictures = new List<Picture>()
            };
            var picture1 = new Picture
            {
                DateCaptured = DateTime.UtcNow,
                PictureData = new PictureData
                {
                    Data = floorPlanBytes
                }
            };
            var picture2 = new Picture
            {
                DateCaptured = DateTime.UtcNow,
                PictureData = new PictureData
                {
                    Data = floorPlanBytes
                }
            };
            defectSpot1.AttachedPictures.Add(picture1);
            defectSpot2.AttachedPictures.Add(picture2);

            checklistItem1.DefectionSpots.Add(defectSpot1);
            checklistItem2.DefectionSpots.Add(defectSpot2);

            user.Projects.Add(project);
            user.ProjectOwned = project;
            customer.Users.Add(user);
            customer.Projects.Add(project);

            _rootContext.Projects.Add(project);
            _rootContext.Users.Add(user);
            _rootContext.Customers.Add(customer);
            _rootContext.Checklists.Add(checklist);
            _rootContext.ChecklistItems.Add(checklistItem1);
            _rootContext.ChecklistItems.Add(checklistItem2);
            _rootContext.ChecklistTemplates.Add(checklistTemplate);
            _rootContext.ChecklistItemTemplates.Add(checklistItemTemplate1);
            _rootContext.ChecklistItemTemplates.Add(checklistItemTemplate2);
            _rootContext.SaveChanges();

            return Json(new { ok = true}, JsonRequestBehavior.AllowGet);
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