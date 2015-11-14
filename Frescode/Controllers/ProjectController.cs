using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
        public async Task<ActionResult> GetBreadcrumbText(int projectId)
        {
            var project = await _rootContext.Projects
                .SingleOrDefaultAsync(x => x.Id == projectId);
            return Json(new { Text = project?.Name }, JsonRequestBehavior.AllowGet);
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
            var cookie = new HttpCookie("test_cookie")
            {
                Value = DateTime.Now.ToString("dd.MM.yyyy"),
                Expires = DateTime.Now.AddMinutes(10),
            };
            Response.SetCookie(cookie);

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
            
            var user1 = new User
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
            #region user 1
            var user2 = new User
            {
                FirstName = "Paulus",
                LastName = "Mikkola",
                DateCreated = DateTime.UtcNow,
                Username = "pavelbaykov89",
                Password = "123456",
                UserRole = UserRole.Reader,
                Projects = new List<Project>(),
                Customer = customer
            };
            #endregion

            var project1 = new Project
            {
                Customer = customer,
                DateCreated = DateTime.UtcNow,
                Owner = user1,
                CreatedBy = user1,
                ChangedBy = user1,
                DateOfLastChange = DateTime.UtcNow,
                Name = "My First Project",
                Checklists = new List<Checklist>(),
                Members = new List<User> {user1},
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
                CreatedBy = user1,
                DateCreated = DateTime.Now,
                Name = "Inspection Drawing 1",
                Size = 1024,
                InspectionDrawingData = inspectionDrawingData
            };

            var checklistTemplate = new ChecklistTemplate
            {
                Name = "Equipment 1",
                Items = new List<ChecklistItemTemplate>(),
                Descendants = new List<Checklist>()
            };
            var checklistTemplate2 = new ChecklistTemplate
            {
                Name = "Equipment 2",
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
                Descendants = new List<ChecklistItem>(),
                InspectionDrawing = inspectionDrawing
            };

            var checklistItemTemplate3 = new ChecklistItemTemplate
            {
                Name = "Equipment 2 -> Item1",
                Checklist = checklistTemplate2,
                Description = "Description of Item 1",
                OrderNumber = 1,
                Descendants = new List<ChecklistItem>(),
                InspectionDrawing = inspectionDrawing
            };
            var checklistItemTemplate4 = new ChecklistItemTemplate
            {
                Name = "Equipment 2 -> Item1",
                Checklist = checklistTemplate2,
                Description = "Description of Item 2",
                OrderNumber = 2,
                Descendants = new List<ChecklistItem>(),
                InspectionDrawing = inspectionDrawing
            };
            var checklistItemTemplate5 = new ChecklistItemTemplate
            {
                Name = "Equipment 2 -> Item 3",
                Checklist = checklistTemplate2,
                Description = "Description of Item 3",
                OrderNumber = 3,
                Descendants = new List<ChecklistItem>(),
                InspectionDrawing = inspectionDrawing
            };
            
            /*#region  Add checklistItemTemplates to checklistTemplate

            for (int i = 3; i < 30; i++)
            {
                var checklistItemTemplate_tmp = new ChecklistItemTemplate
                    {
                        Name = "Item"+i.ToString(),
                        Checklist = checklistTemplate,
                        Description = "Description of Item"+i.ToString(),
                        OrderNumber = i,
                        Descendants = new List<ChecklistItem>(),
                        InspectionDrawing = inspectionDrawing
                    };
                checklistTemplate.Items.Add(checklistItemTemplate_tmp);
                if (i%2 == 0)
                {
                    checklistTemplate2.Items.Add(checklistItemTemplate_tmp);
                }
            }
            #endregion*/
            checklistTemplate.Items.Add(checklistItemTemplate1);
            checklistTemplate.Items.Add(checklistItemTemplate2);
            checklistTemplate2.Items.Add(checklistItemTemplate3);
            checklistTemplate2.Items.Add(checklistItemTemplate4);
            checklistTemplate2.Items.Add(checklistItemTemplate5);
            var checklist1 = new Checklist
            {
                Project = project1,
                ChangedBy = user1,
                DateCreated = DateTime.UtcNow,
                DateOfLastChange = DateTime.UtcNow,
                CreatedBy = user1,
                ChecklistTemplate = checklistTemplate,
                Items = new List<ChecklistItem>()
            };
            var checklist2 = new Checklist
            {
                Project = project1,
                ChangedBy = user2,
                DateCreated = DateTime.UtcNow,
                DateOfLastChange = DateTime.UtcNow,
                CreatedBy = user1,
                ChecklistTemplate = checklistTemplate,
                Items = new List<ChecklistItem>()
            };
            var checklist3 = new Checklist
            {
                Project = project1,
                ChangedBy = user2,
                DateCreated = DateTime.UtcNow,
                DateOfLastChange = DateTime.UtcNow,
                CreatedBy = user1,
                ChecklistTemplate = checklistTemplate2,
                Items = new List<ChecklistItem>()
            };
            checklistTemplate.Descendants.Add(checklist1);
            checklistTemplate.Descendants.Add(checklist2);
            checklistTemplate2.Descendants.Add(checklist3);

            project1.Checklists.Add(checklist1);
            project1.Checklists.Add(checklist2);
            project1.Checklists.Add(checklist3);

            var checklistItem1 = new ChecklistItem
            {
                ChangedBy = user1,
                Checklist = checklist1,
                ItemTemplate = checklistItemTemplate1,
                Status = ChecklistItemStatus.NotCompleted,
                DateOfLastChange = DateTime.UtcNow,
                DefectionSpots = new List<DefectionSpot>()
            };
            var checklistItem2 = new ChecklistItem
            {
                ChangedBy = user1,
                Checklist = checklist1,
                ItemTemplate = checklistItemTemplate2,
                Status = ChecklistItemStatus.Completed,
                DateOfLastChange = DateTime.UtcNow,
                DefectionSpots = new List<DefectionSpot>()
            };

            checklistItemTemplate1.Descendants.Add(checklistItem1);
            checklistItemTemplate2.Descendants.Add(checklistItem2);
            checklist1.Items.Add(checklistItem1);
            checklist1.Items.Add(checklistItem2);

            checklist2.ChecklistInit();
            checklist3.ChecklistInit();

            var defectSpot1 = new DefectionSpot
            {
                ChecklistItem = checklistItem1,
                DateCreated = DateTime.UtcNow,
                Description = "Description of defect spot",
                X = 0.2,
                Y = 0.5,
                OrderNumber = 1,
                AttachedPictures = new List<Picture>()
            };
            var defectSpot2 = new DefectionSpot
            {
                ChecklistItem = checklistItem2,
                DateCreated = DateTime.UtcNow,
                Description = "Description of defect spot",
                X = 0.3,
                Y = 0.1,
                OrderNumber = 1,
                AttachedPictures = new List<Picture>()
            };
            var picture1 = new Picture
            {
                DateCaptured = DateTime.UtcNow,
                Name = "Picture 1",
                PictureData = new PictureData
                {
                    Data = floorPlanBytes
                }
            };
            var picture2 = new Picture
            {
                DateCaptured = DateTime.UtcNow,
                Name = "Picture 2",
                PictureData = new PictureData
                {
                    Data = floorPlanBytes
                }
            };
            defectSpot1.AttachedPictures.Add(picture1);
            defectSpot2.AttachedPictures.Add(picture2);

            checklistItem1.DefectionSpots.Add(defectSpot1);
            checklistItem2.DefectionSpots.Add(defectSpot2);

            _rootContext.DefectionSpots.Add(defectSpot1);
            _rootContext.DefectionSpots.Add(defectSpot2);

            user1.Projects.Add(project1);
            user1.ProjectOwned = project1;
            customer.Users.Add(user1);
            customer.Projects.Add(project1);

            _rootContext.Projects.Add(project1);
            _rootContext.Users.Add(user1);
            _rootContext.Customers.Add(customer);
            _rootContext.Checklists.Add(checklist1);
            
            _rootContext.ChecklistItems.Add(checklistItem1);
            _rootContext.ChecklistItems.Add(checklistItem2);
            _rootContext.ChecklistTemplates.Add(checklistTemplate);
            _rootContext.ChecklistTemplates.Add(checklistTemplate2);
            _rootContext.ChecklistItemTemplates.Add(checklistItemTemplate1);
            _rootContext.ChecklistItemTemplates.Add(checklistItemTemplate2);
            _rootContext.ChecklistItemTemplates.Add(checklistItemTemplate3);
            _rootContext.ChecklistItemTemplates.Add(checklistItemTemplate4);
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