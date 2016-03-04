using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using DALLib;
using DALLib.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;

namespace DALLib.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DALLib.RootContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DALLib.RootContext context)
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

            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            userManager.Create(user1, "123456");
            userManager.Create(user2, "123456");

            user1 = context.Users.Single(x => x.UserName == user1.UserName);
            user2 = context.Users.Single(x => x.UserName == user2.UserName);

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
                Checklists = new List<Checklist>(),
                Members = new List<User> { user1 },
                Status = ProjectStatus.Done
            };

            var webClient = new WebClient();
            var floorPlanBytes = webClient.DownloadData("http://www.hollandaleapartments.com/images/floor%20plans/Embassy-floor-plan.png");

            var httpClient = new HttpClient();
            //var floorPlanBytes = await httpClient.GetByteArrayAsync("");
            //httpClient.GetByteArrayAsync("http://www.hollandaleapartments.com/images/floor%20plans/Embassy-floor-plan.png");
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
            context.ChecklistItems.Add(checklistItem1);
            context.ChecklistItems.Add(checklistItem2);

            //checklist2.ChecklistInit(Context);
            //checklist3.ChecklistInit(Context);

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

            var structures = new List<Structure>(){
                new Structure() { Path = "Folder1/", Name = "", Project = project1 },
                new Structure() { Path = "Folder2/", Name = "", Project = project1 },
                new Structure() { Path = "Folder1/Folder3/", Name = "", Project = project1 },
                new Structure() { Path = "Folder1/", Name = "InspectionDrawing", Project = project1}
            };

            context.Structures.AddRange(structures);

            context.DefectionSpots.Add(defectSpot1);
            context.DefectionSpots.Add(defectSpot2);

            user1.Projects.Add(project1);
            user1.ProjectsOwned.Add(project1);
            customer.Users.Add(user1);
            customer.Projects.Add(project1);

            context.Projects.Add(project1);
            context.Customers.Add(customer);
            context.Checklists.Add(checklist1);

            context.ChecklistItems.Add(checklistItem1);
            context.ChecklistItems.Add(checklistItem2);
            context.ChecklistTemplates.Add(checklistTemplate);
            context.ChecklistTemplates.Add(checklistTemplate2);
            context.ChecklistItemTemplates.Add(checklistItemTemplate1);
            context.ChecklistItemTemplates.Add(checklistItemTemplate2);
            context.ChecklistItemTemplates.Add(checklistItemTemplate3);
            context.ChecklistItemTemplates.Add(checklistItemTemplate4);

            context.SaveChanges();
        }
    }
}
