using System.Linq;
using System.Threading.Tasks;
using Frescode.BL.Commands;
using DALLib;
using DALLib.Entities;
using MediatR;

namespace Frescode.BL.CommandsHandler
{
    public class InspectionDrawingChecklistItemChangeStateCommandHandler : IAsyncNotificationHandler <InspectionDrawingChecklistItemChangeStateCommand>
    {
        private readonly RootContext _rootContext;

        public InspectionDrawingChecklistItemChangeStateCommandHandler(RootContext rootContext)
        {
            _rootContext = rootContext;
        }

        public async Task Handle(InspectionDrawingChecklistItemChangeStateCommand notification)
        {
            var item = _rootContext.ChecklistItemsForInspectionDrawing.Single(x => x.Id == notification.ItemId);
            item.Status = notification.State ? ChecklistItemStatus.Completed : ChecklistItemStatus.NotCompleted;
            await _rootContext.SaveChangesAsync();
        }
    }
}