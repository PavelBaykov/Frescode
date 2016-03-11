using System;
using System.Collections.Generic;
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
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;

namespace Frescode.Controllers
{
    [AllowAnonymous]
    public class InitDbController : BaseController
    {
        public InitDbController(IMediator mediator, RootContext rootContext) : base(mediator, rootContext)
        {
        }

        [HttpGet]
        public async Task<ActionResult> InitDb()
        {
            var user1 = new User
            {
                FirstName = "Pavel",
                LastName = "Baykov",
                DateCreated = DateTime.UtcNow,
                UserName = "pavelbaykov89@gmail.com",
                Email = "pavelbaykov89@gmail.com",
                UserRole = UserRole.Reader,
                Projects = new List<Project>(),
                ProjectsOwned = new List<Project>()
            };
            var user2 = new User
            {
                FirstName = "Paulus",
                LastName = "Mikkola",
                DateCreated = DateTime.UtcNow,
                UserName = "paulus@gmail.com",
                Email = "paulus@gmail.com",
                UserRole = UserRole.Reader,
                Projects = new List<Project>(),
            };

            var userStore = new UserStore<User>(Context);
            var userManager = new UserManager<User>(userStore);

            userManager.Create(user1, "123456");
            userManager.Create(user2, "123456");

            user1 = Context.Users.Single(x => x.UserName == user1.UserName);
            user2 = Context.Users.Single(x => x.UserName == user2.UserName);

            var customer = new Customer
            {
                Projects = new List<Project>(),
                Users = new List<User>()
            };


            var project1 = new Project
            {
                Customer = customer,
                DateCreated = DateTime.UtcNow,
                Owner = user1,
                CreatedBy = user1,
                ChangedBy = user1,
                DateOfLastChange = DateTime.UtcNow,
                Name = "My First Project",
                Checklists = new List<ChecklistForProject>(),
                Members = new List<User> { user1 },
                Status = ProjectStatus.Done
            };

            var webClient = new WebClient();
            var floorPlanBytes = webClient.DownloadData("http://www.hollandaleapartments.com/images/floor%20plans/Embassy-floor-plan.png");
            var inspectionDrawingTemplate = new InspectionDrawingTemplate
            {
                Data = floorPlanBytes,
                Name = "InspectionDrawing1"
            };

            var inspectionDrawing = new InspectionDrawing
            {
                InspectionDrawingTemplate = inspectionDrawingTemplate,
                Checklists = new List<ChecklistForInspectionDrawing>(),
                DefectionSpots = new List<DefectionSpot>()
            };



            var checklistTemplate = new ChecklistTemplate
            {
                Name = "Equipment 1",
                Items = new List<ChecklistItemTemplate>(),
                DescendantsForProject = new List<ChecklistForProject>(),
                DescendantsForInspectionDrawing = new List<ChecklistForInspectionDrawing>()
            };
            var checklistTemplate2 = new ChecklistTemplate
            {
                Name = "Equipment 2",
                Items = new List<ChecklistItemTemplate>(),
                DescendantsForProject = new List<ChecklistForProject>(),
                DescendantsForInspectionDrawing = new List<ChecklistForInspectionDrawing>()
            };
            var checklistItemTemplate1 = new ChecklistItemTemplate
            {
                Name = "Item 1",
                Checklist = checklistTemplate,
                Description = "Description of Item 1",
                OrderNumber = 2,
                DescendantsForInspectionDrawing = new List<ChecklistItemForInspectionDrawing>(),
                DescendantsForProject = new List<ChecklistItemForProject>()
            };

            var checklistItemTemplate2 = new ChecklistItemTemplate
            {
                Name = "Item 2",
                Checklist = checklistTemplate,
                Description = "Description of Item 2",
                OrderNumber = 1,
                DescendantsForInspectionDrawing = new List<ChecklistItemForInspectionDrawing>(),
                DescendantsForProject = new List<ChecklistItemForProject>()
            };

            var checklistItemTemplate3 = new ChecklistItemTemplate
            {
                Name = "Equipment 2 -> Item1",
                Checklist = checklistTemplate2,
                Description = "Description of Item 1",
                OrderNumber = 1,
                DescendantsForInspectionDrawing = new List<ChecklistItemForInspectionDrawing>(),
                DescendantsForProject = new List<ChecklistItemForProject>()
            };
            var checklistItemTemplate4 = new ChecklistItemTemplate
            {
                Name = "Equipment 2 -> Item1",
                Checklist = checklistTemplate2,
                Description = "Description of Item 2",
                OrderNumber = 2,
                DescendantsForInspectionDrawing = new List<ChecklistItemForInspectionDrawing>(),
                DescendantsForProject = new List<ChecklistItemForProject>()
            };
            var checklistItemTemplate5 = new ChecklistItemTemplate
            {
                Name = "Equipment 2 -> Item 3",
                Checklist = checklistTemplate2,
                Description = "Description of Item 3",
                OrderNumber = 3,
                DescendantsForInspectionDrawing = new List<ChecklistItemForInspectionDrawing>(),
                DescendantsForProject = new List<ChecklistItemForProject>()
            };


            checklistTemplate.Items.Add(checklistItemTemplate1);
            checklistTemplate.Items.Add(checklistItemTemplate2);
            checklistTemplate2.Items.Add(checklistItemTemplate3);
            checklistTemplate2.Items.Add(checklistItemTemplate4);
            checklistTemplate2.Items.Add(checklistItemTemplate5);

            var idchecklist1 = new ChecklistForInspectionDrawing()
            {
                ChangedBy = user1,
                ChecklistTemplate = checklistTemplate,
                CreatedBy = user1,
                DateCreated = DateTime.UtcNow,
                DateOfLastChange = DateTime.UtcNow,
                InspectionDrawing = inspectionDrawing,
                Items = new List<ChecklistItemForInspectionDrawing>()
            };

            var idchecklist2 = new ChecklistForInspectionDrawing()
            {
                ChangedBy = user1,
                ChecklistTemplate = checklistTemplate2,
                CreatedBy = user1,
                DateCreated = DateTime.UtcNow,
                DateOfLastChange = DateTime.UtcNow,
                InspectionDrawing = inspectionDrawing,
                Items = new List<ChecklistItemForInspectionDrawing>()
            };

            var idchecklistItem1 = new ChecklistItemForInspectionDrawing()
            {
                ChangedBy = user1,
                DateOfLastChange = DateTime.UtcNow,
                Checklist = idchecklist1,
                ItemTemplate = checklistItemTemplate1,
                Status = ChecklistItemStatus.Completed
            };

            var idchecklistItem2 = new ChecklistItemForInspectionDrawing()
            {
                ChangedBy = user1,
                DateOfLastChange = DateTime.UtcNow,
                Checklist = idchecklist1,
                ItemTemplate = checklistItemTemplate2,
                Status = ChecklistItemStatus.Completed
            };

            var idchecklistItem3 = new ChecklistItemForInspectionDrawing()
            {
                ChangedBy = user1,
                DateOfLastChange = DateTime.UtcNow,
                Checklist = idchecklist2,
                ItemTemplate = checklistItemTemplate3,
                Status = ChecklistItemStatus.Completed
            };

            var idchecklistItem4 = new ChecklistItemForInspectionDrawing()
            {
                ChangedBy = user1,
                DateOfLastChange = DateTime.UtcNow,
                Checklist = idchecklist2,
                ItemTemplate = checklistItemTemplate4,
                Status = ChecklistItemStatus.Completed
            };

            idchecklist1.Items.Add(idchecklistItem1);
            idchecklist1.Items.Add(idchecklistItem2);
            idchecklist2.Items.Add(idchecklistItem3);
            idchecklist2.Items.Add(idchecklistItem4);



            var checklist1 = new ChecklistForProject
            {
                Project = project1,
                ChangedBy = user1,
                DateCreated = DateTime.UtcNow,
                DateOfLastChange = DateTime.UtcNow,
                CreatedBy = user1,
                ChecklistTemplate = checklistTemplate,
                Items = new List<ChecklistItemForProject>()
            };
            var checklist2 = new ChecklistForProject
            {
                Project = project1,
                ChangedBy = user2,
                DateCreated = DateTime.UtcNow,
                DateOfLastChange = DateTime.UtcNow,
                CreatedBy = user1,
                ChecklistTemplate = checklistTemplate,
                Items = new List<ChecklistItemForProject>()
            };
            var checklist3 = new ChecklistForProject
            {
                Project = project1,
                ChangedBy = user2,
                DateCreated = DateTime.UtcNow,
                DateOfLastChange = DateTime.UtcNow,
                CreatedBy = user1,
                ChecklistTemplate = checklistTemplate2,
                Items = new List<ChecklistItemForProject>()
            };
            checklistTemplate.DescendantsForProject.Add(checklist1);
            checklistTemplate.DescendantsForProject.Add(checklist2);
            checklistTemplate2.DescendantsForProject.Add(checklist3);

            project1.Checklists.Add(checklist1);
            project1.Checklists.Add(checklist2);
            project1.Checklists.Add(checklist3);

            var checklistItem1 = new ChecklistItemForProject
            {
                ChangedBy = user1,
                Checklist = checklist1,
                ItemTemplate = checklistItemTemplate1,
                Status = ChecklistItemStatus.NotCompleted,
                DateOfLastChange = DateTime.UtcNow,
                
            };
            var checklistItem2 = new ChecklistItemForProject
            {
                ChangedBy = user1,
                Checklist = checklist1,
                ItemTemplate = checklistItemTemplate2,
                Status = ChecklistItemStatus.Completed,
                DateOfLastChange = DateTime.UtcNow,
                
            };

            checklistItemTemplate1.DescendantsForProject.Add(checklistItem1);
            checklistItemTemplate2.DescendantsForProject.Add(checklistItem2);
            checklist1.Items.Add(checklistItem1);
            checklist1.Items.Add(checklistItem2);
            Context.ChecklistItemsForProject.Add(checklistItem1);
            Context.ChecklistItemsForProject.Add(checklistItem2);

            //checklist2.ChecklistInit(Context.);
            //checklist3.ChecklistInit(Context.);

            //var defectSpot1 = new DefectionSpot
            //{
            //    ChecklistItem = checklistItem1,
            //    DateCreated = DateTime.UtcNow,
            //    Description = "Description of defect spot",
            //    X = 0.2,
            //    Y = 0.5,
            //    OrderNumber = 1,
            //    AttachedPictures = new List<Picture>()
            //};
            //var defectSpot2 = new DefectionSpot
            //{
            //    ChecklistItem = checklistItem2,
            //    DateCreated = DateTime.UtcNow,
            //    Description = "Description of defect spot",
            //    X = 0.3,
            //    Y = 0.1,
            //    OrderNumber = 1,
            //    AttachedPictures = new List<Picture>()
            //};
            //var picture1 = new Picture
            //{
            //    DateCaptured = DateTime.UtcNow,
            //    Name = "Picture 1",
            //    PictureData = new PictureData
            //    {
            //        Data = floorPlanBytes
            //    }
            //};
            //var picture2 = new Picture
            //{
            //    DateCaptured = DateTime.UtcNow,
            //    Name = "Picture 2",
            //    PictureData = new PictureData
            //    {
            //        Data = floorPlanBytes
            //    }
            //};
            //defectSpot1.AttachedPictures.Add(picture1);
            //defectSpot2.AttachedPictures.Add(picture2);

            //checklistItem1.DefectionSpots.Add(defectSpot1);
            //checklistItem2.DefectionSpots.Add(defectSpot2);

            var structures = new List<Structure>(){
                new Structure() { Path = "Folder1/", Name = "", Project = project1,InspectionDrawing = null },
                new Structure() { Path = "Folder2/", Name = "", Project = project1,InspectionDrawing = null },
                new Structure() { Path = "Folder1/Folder3/", Name = "", Project = project1,InspectionDrawing = null },
                new Structure() { Path = "Folder1/", Name = "InspectionDrawing", Project = project1, InspectionDrawing = inspectionDrawing}
            };

            Context.Structures.AddRange(structures);

            //Context.DefectionSpots.Add(defectSpot1);
            //Context.DefectionSpots.Add(defectSpot2);

            user1.Projects.Add(project1);
            user1.ProjectsOwned.Add(project1);
            customer.Users.Add(user1);
            customer.Projects.Add(project1);

            Context.Projects.Add(project1);
            Context.Customers.Add(customer);
            Context.ChecklistsForProject.Add(checklist1);


            Context.InspectionDrawings.Add(inspectionDrawing);
            Context.InspectionDrawingTemplate.Add(inspectionDrawingTemplate);
            Context.ChecklistItemsForProject.Add(checklistItem1);
            Context.ChecklistItemsForProject.Add(checklistItem2);
            Context.ChecklistTemplates.Add(checklistTemplate);
            Context.ChecklistTemplates.Add(checklistTemplate2);
            Context.ChecklistItemTemplates.Add(checklistItemTemplate1);
            Context.ChecklistItemTemplates.Add(checklistItemTemplate2);
            Context.ChecklistItemTemplates.Add(checklistItemTemplate3);
            Context.ChecklistItemTemplates.Add(checklistItemTemplate4);

            Context.ChecklistsForInspectionDrawing.Add(idchecklist1);
            Context.ChecklistsForInspectionDrawing.Add(idchecklist2);
            Context.ChecklistItemsForInspectionDrawing.Add(idchecklistItem1);
            Context.ChecklistItemsForInspectionDrawing.Add(idchecklistItem2);
            Context.ChecklistItemsForInspectionDrawing.Add(idchecklistItem3);
            Context.ChecklistItemsForInspectionDrawing.Add(idchecklistItem4);


            Context.SaveChanges();
            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
        }
    }
}