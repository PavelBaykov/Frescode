using DALLib;
using DALLib.Entities;
using Frescode.BL.Commands;
using MediatR;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Frescode.Controllers
{
    public class InspectionDrawingScreenController : BaseController
    {
        public InspectionDrawingScreenController(IMediator mediator, RootContext rootContext)
            : base(mediator, rootContext)
        {
        }

        // GET: InspectionDrawingScreen
        public ActionResult InspectionDrawingsDetails(int projectId, int drawingId)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.DrawingId = drawingId;
            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetBreadcrumbText(int drawingId)
        {
            var inspectionDrawing = await Context.InspectionDrawings
                .Include(drawing => drawing.InspectionDrawingTemplate)
                .SingleOrDefaultAsync(drawing => drawing.Id == drawingId);

            return Json(new { Text = inspectionDrawing.InspectionDrawingTemplate?.Name }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetChecklistsList(int drawingId)
        {
            var inspectionDrawing = Context.InspectionDrawings
                .Include(drawing => drawing.Checklists)
                .Include(drawing => drawing.Checklists.Select(ch => ch.ChecklistTemplate))
                .Include(drawing => drawing.Checklists.Select(ch => ch.Items))
                .Include(drawing => drawing.Checklists.Select(ch => ch.ChangedBy))
                .SingleOrDefault(drawing => drawing.Id == drawingId);
            var viewModel = new ProjectChecklistsListViewModel();
            foreach (var checklist in inspectionDrawing.Checklists)
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
        public ActionResult GetInspectionDrawingDetails(int drawingId)
        {
            var inspectionDrawingDetails = Context.InspectionDrawings
                .Include(drawing => drawing.InspectionDrawingTemplate)
                .Include(drawing => drawing.DefectionSpots.
                    Select(spot => spot.AttachedPictures.Select(picture => picture.PictureData)))
                .SingleOrDefault(drawing => drawing.Id == drawingId);

            var viewModel = new InspectionDrawingDetailsViewModel();
            viewModel.Id = drawingId;
            viewModel.InspectionDrawingPath =
                $"/InspectionDrawingScreen/GetInspectionDrawing?inspectionDrawingId={inspectionDrawingDetails.InspectionDrawingTemplate.Id}";
            foreach (var spot in inspectionDrawingDetails.DefectionSpots)
            {
                var spotViewModel = new DefectSpotViewModel();
                spotViewModel.Id = spot.Id;
                spotViewModel.Description = spot.Description;
                spotViewModel.OrderNumber = spot.OrderNumber;
                spotViewModel.X = spot.X;
                spotViewModel.Y = spot.Y;
                spotViewModel.AttachedPictures =
                        spot
                            .AttachedPictures
                            .Select(x => new AttachedPictureViewModel{ FullPath = $"/Picture/GetPicture?pictureId={x.Id}", Thumb = $"/Picture/GetPictureThumbnailForDefectSpot?pictureId={x.Id}" })
                            .ToList();
                viewModel.DefectSpotsList.Add(spotViewModel);
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInspectionDrawing(int inspectionDrawingId)
        {
            var inspectionDrawing = Context.InspectionDrawingTemplate.SingleOrDefault(x => x.Id == inspectionDrawingId);
            return File(inspectionDrawing.Data, "image/png");
        }

        [HttpGet]
        public ActionResult DeleteDefectSpot(int defectSpotId)
        {
            Context.DefectionSpots.Remove(Context.DefectionSpots.Single(x => x.Id == defectSpotId));
            Context.SaveChanges();
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        public async Task<ActionResult> AddDefectSpot(SpotDto spotDto)
        {

        var addDefectCommand = new AddDefectSpotCommand(spotDto.Id, spotDto.Description, spotDto.OrderNumber, spotDto.X, spotDto.Y,
                spotDto.InspectionDrawingId);
            await Mediator.PublishAsync(addDefectCommand);
            return Json(new { Id = addDefectCommand.Id });
        }
    }

    public class SpotDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int InspectionDrawingId { get; set; }
    }

    public class InspectionDrawingDetailsViewModel
    {
        public InspectionDrawingDetailsViewModel()
        {
            DefectSpotsList = new List<DefectSpotViewModel>();
        }
        public int Id { get; set; }
        public List<DefectSpotViewModel> DefectSpotsList { get; set; }
        public string InspectionDrawingPath { get; set; }
    }

    public class DefectSpotViewModel
    {
        public DefectSpotViewModel()
        {
            AttachedPictures = new List<AttachedPictureViewModel>();
        }
        public int Id { get; set; }
        public List<AttachedPictureViewModel> AttachedPictures { get; set; }
        public int OrderNumber { get; set; }
        public string Description { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class AttachedPictureViewModel{
        public string FullPath { get; set; }
        public string Thumb { get; set; }
    }



}