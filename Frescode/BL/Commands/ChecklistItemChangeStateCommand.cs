using MediatR;

namespace Frescode.BL.Commands
{
    public class ChecklistItemChangeStateCommand : IAsyncNotification
    {
        public int ItemId { get; }
        public bool State { get; }

        public ChecklistItemChangeStateCommand(int itemId, bool state)
        {
            ItemId = itemId;
            State = state;
        }
    }
}
