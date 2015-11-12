using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Frescode.BL.Commands;
using Frescode.DAL;
using Frescode.DAL.Entities;
using MediatR;
using WebGrease.Css.Ast.Selectors;

namespace Frescode.Controllers
{
    public class ChecklistItemsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly RootContext _rootContext;

        public ChecklistItemsController(IMediator mediator, RootContext rootContext)
        {
            _mediator = mediator;
            _rootContext = rootContext;
        }

        public async Task<ActionResult> GetBreadcrumbText(int checklistItemId)
        {
            var checklistItem = await _rootContext.ChecklistItems
                .Include(x => x.ItemTemplate)
                .SingleOrDefaultAsync(x => x.Id == checklistItemId);
            return Json(new { Text = checklistItem?.ItemTemplate?.Name }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChecklistItemsList(int userId, int checklistId)
        {
            ViewBag.ChecklistId = checklistId;
            ViewBag.UserId = userId;
            return View();
        }

        public ActionResult ChecklistItemDetails(int userId, int checklistItemId)
        {
            ViewBag.ChecklistItemId = checklistItemId;
            ViewBag.UserId = userId;
            return View();
        }

        public ActionResult DefectSpotAddition(int userId, int checklistItemId)
        {
            ViewBag.UserId = userId;
            ViewBag.ChecklistItemId = checklistItemId;
            return View();
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
            await _mediator.PublishAsync(addDefectCommand);
            return Json(new { defectSpotId = addDefectCommand.Id});
        }


        [HttpGet]
        public ActionResult GetChecklistItemsList(int userId, int checklistId)
        {
            var checklist = _rootContext.Checklists
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
        public ActionResult GetChecklistItemDetails(int userId, int checklistItemId)
        {
            var checklistItem = _rootContext.ChecklistItems
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
                defectSpotViewModel.AttachedPictures = string.Empty;
                foreach (var attachedPicture in defectSpot.AttachedPictures)
                {
                    defectSpotViewModel.AttachedPictures +=
                        $"/Picture/GetPicture?pictureId={attachedPicture.PictureId},";
                }
                defectSpotViewModel.AttachedPictures = defectSpotViewModel.AttachedPictures.TrimEnd(',');
                viewModel.DefectSpotsList.Add(defectSpotViewModel);
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ChecklistItemSet(int checklistItemId, bool newState)
        {
            await _mediator.PublishAsync(new ChecklistItemChangeStateCommand(checklistItemId, newState));

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
        public string AttachedPictures { get; set; }
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
     