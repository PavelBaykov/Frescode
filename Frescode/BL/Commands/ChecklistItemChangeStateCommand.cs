using MediatR;

namespace Frescode.BL.Commands
{
    public class ProjectChecklistItemChangeStateCommand : IAsyncNotification
    {
        public int ItemId { get; }
        public bool State { get; }

        public ProjectChecklistItemChangeStateCommand(int itemId, bool state)
        {
            ItemId = itemId;
            State = state;
        }
    }
}
