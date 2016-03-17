
using MediatR;

namespace Frescode.BL.Commands
{
    public class InspectionDrawingChecklistItemChangeStateCommand : IAsyncNotification
    {
        public int ItemId { get; }
        public bool State { get; }

        public InspectionDrawingChecklistItemChangeStateCommand(int itemId, bool state)
        {
            ItemId = itemId;
            State = state;
        }
    }
}
