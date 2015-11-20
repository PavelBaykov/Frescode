using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frescode.BL.Commands;
using Frescode.DAL;
using Frescode.DAL.Entities;
using MediatR;

namespace Frescode.Controllers
{
    [Authorize]
    public class ChecklistItemsController : BaseController
    {
        public ChecklistItemsController(IMediator mediator, RootContext rootContext)
            : base(mediator, rootContext)
        {
        }

        public async Task<ActionResult> GetBreadcrumbText(int checklistItemId)
        {
            var checklistItem = await Context.ChecklistItems
                .Include(x => x.ItemTemplate)
                .SingleOrDefaultAsync(x => x.Id == checklistItemId);
            return Json(new { Text = checklistItem?.ItemTemplate?.Name }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetBreadcrumbDefectSpotText(int defectSpotId)
        {
            var defectSpot = await Context.DefectionSpots
                .SingleOrDefaultAsync(x => x.Id == defectSpotId);
            return Json(new { Text = $"Defect spot #{defectSpot.OrderNumber}" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChecklistItemsList(int checklistId)
        {
            ViewBag.ChecklistId = checklistId;
            ViewBag.UserId = User.Identity.Name;
            return View();
        }

        public ActionResult ChecklistItemDetails(int checklistItemId)
        {
            ViewBag.ChecklistItemId = checklistItemId;
            ViewBag.UserId = User.Identity.Name;
            return View();
        }

        public class AddSpotViewModel
        {
            public AddSpotViewModel()
            {
                AttachedPictures = new List<PictureViewModel>();
            }

            public int Id { get; set; }
            public int OrderNumber { get; set; }
            public string Description { get; set; }
            public List<PictureViewModel> AttachedPictures { get; set; }
        }
        public class PictureViewModel
        {
            public string Path { get; set; }
            public string Timestamp { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult> GetDefectSpot(string userId, int defectSpotId)
        {
            var defectSpot = await Context.DefectionSpots
                .Include(x => x.AttachedPictures)
                .SingleOrDefaultAsync(x => x.Id == defectSpotId);

            var viewModel = new AddSpotViewModel
            {
                Id = defectSpot.Id,
                OrderNumber = defectSpot.OrderNumber,
                Description = defectSpot.Description
            };
            foreach (var picture in defectSpot.AttachedPictures)
            {
                var pictureViewModel = new PictureViewModel
                {
                    Path = $"/Picture/GetPicture?pictureId={picture.PictureId}",
                    Timestamp = picture.DateCaptured.ToShortDateString()
                };

                viewModel.AttachedPictures.Add(pictureViewModel);
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DefectSpotAddition(int defectSpotId)
        {
            ViewBag.UserId = User.Identity.Name;
            ViewBag.DefectSpotId = defectSpotId;
            return View();
        }

        [HttpPost]
        public ActionResult UploadAddSpotImage(int defectSpotId)
        {
            return Json(new {});
        }


        public class DescriptionDto
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }
        [HttpPost]
        public ActionResult SaveChecklistItemDescription(DescriptionDto dto)
        {
            string defectSpotDescription = dto.Description;
            int defectSpotId = dto.Id;
            var defectSpot = Context.DefectionSpots.Single(x => x.Id == defectSpotId);
            defectSpot.Description = defectSpotDescription;
            Context.SaveChanges();
            return Json(new {});
        }


        public class SpotDto
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int OrderNumber{ get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public int ChecklistItemId { get; set; }
        }
        [HttpPost]
        public async Task<ActionResult> AddDefectSpot(SpotDto spotDto)
        {
            var addDefectCommand = new AddDefectSpotCommand(spotDto.Id, spotDto.Description, spotDto.OrderNumber, spotDto.X, spotDto.Y,
                spotDto.ChecklistItemId);
            await Mediator.PublishAsync(addDefectCommand);
            return Json(new { Id = addDefectCommand.Id});
        }

        [HttpGet]
        public ActionResult DeleteDefectSpot(int defectSpotId)
        {
            Context.DefectionSpots.Remove(Context.DefectionSpots.Single(x => x.Id == defectSpotId));
            Context.SaveChanges();
            return Json(new {}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetChecklistItemsList(int checklistId)
        {
            var checklist = Context.Checklists
                .Include(x => x.Items.Select(w => w.ItemTemplate))
                .Include(x => x.Items.Select(w => w.ChangedBy))
                .Include(x => x.Items.Select(w => w.DefectionSpots))
                .Include(x => x.ChecklistTemplate)
                .Include(x => x.Project)
                .SingleOrDefault(x => x.Id == checklistId);

            var viewModel = new ChecklistItemsListViewModel();
            viewModel.ChecklistName = checklist.ChecklistTemplate.Name;
            viewModel.ChecklistProjectName = checklist.Project.Name;
            viewModel.ChecklistProjectId = checklist.Project.Id;
            foreach (var item in checklist.Items)
            {
                var checklistItemViewModel = new ChecklistItemViewModel
                {
                    Id = item.Id,
                    Name = item.ItemTemplate.Name,
                    ChangedBy = $"{item.ChangedBy.FirstName} {item.ChangedBy.LastName}",
                    DateOfLastChange = item.DateOfLastChange.ToString("MM/dd/yy H:mm:ss"),
                    Status = item.Status == ChecklistItemStatus.Completed,
                    DefectSpotsCount = item.DefectionSpots.Count(),
                    Description = item.ItemTemplate.Description
                };
                viewModel.ChecklistItemsList.Add(checklistItemViewModel);
            }
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetChecklistItemDetails(int checklistItemId)
        {
            var checklistItem = Context.ChecklistItems
                .Include(x => x.DefectionSpots.Select(q => q.AttachedPictures))
                .Include(x => x.Checklist.ChecklistTemplate)
                .Include(x => x.Checklist.Project)
                .Include(x => x.ItemTemplate)
                .Include(x => x.ItemTemplate.InspectionDrawing)
                .SingleOrDefault(x => x.Id == checklistItemId);

            var viewModel = new ChecklistItemDescriptionViewModel();
            viewModel.Id = checklistItem.Id;
            viewModel.ChecklistName = checklistItem.Checklist.ChecklistTemplate.Name;
            viewModel.Description = checklistItem.ItemTemplate.Description;
            viewModel.ProjectName = checklistItem.Checklist.Project.Name;
            viewModel.DefectSpotsList = new List<DefectSpotViewModel>();
            viewModel.InspectionDrawingPath = $"/InspectionDrawing/GetInspectionDrawing?inspectionDrawingId={checklistItem.ItemTemplate.InspectionDrawing.InspectionDrawingDataId}";
            foreach (var defectSpot in checklistItem.DefectionSpots)
            {
                var defectSpotViewModel = new DefectSpotViewModel();
                defectSpotViewModel.Id = defectSpot.Id;
                defectSpotViewModel.Description = defectSpot.Description;
                defectSpotViewModel.OrderNumber = defectSpot.OrderNumber;
                defectSpotViewModel.X = defectSpot.X;
                defectSpotViewModel.Y = defectSpot.Y;
                defectSpotViewModel.AttachedPictures =
                    defectSpot
                        .AttachedPictures
                        .Select(x => $"/Picture/GetPicture?pictureId={x.PictureId}")
                        .ToList();
                viewModel.DefectSpotsList.Add(defectSpotViewModel);
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ChecklistItemSet(int checklistItemId, bool newState)
        {
            await Mediator.PublishAsync(new ChecklistItemChangeStateCommand(checklistItemId, newState));

            return Json(new {ok = true}, JsonRequestBehavior.AllowGet);
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
        public int DefectSpotsCount { get; set; }
    }

    public class ChecklistItemDescriptionViewModel
    {
        public int Id { get; set; }
        public List<DefectSpotViewModel> DefectSpotsList { get; set; }
        public string Description { get; set; }
        public string ChecklistName { get; set; }
        public string ProjectName { get; set; }
        public string InspectionDrawingPath { get; set; }
    }

    public class DefectSpotViewModel
    {
        public int Id { get; set; }
        public List<string> AttachedPictures { get; set; }
        public int OrderNumber { get; set; }
        public string Description { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class AddDefectSpotDto
    {
        public string Description { get; set; }
    }
}
     