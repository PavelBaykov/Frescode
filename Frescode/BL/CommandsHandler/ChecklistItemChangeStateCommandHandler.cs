using System.Linq;
using System.Threading.Tasks;
using Frescode.BL.Commands;
using DALLib;
using DALLib.Entities;
using MediatR;

namespace Frescode.BL.CommandsHandler
{
    public class ChecklistItemChangeStateCommandHandler : IAsyncNotificationHandler<ChecklistItemChangeStateCommand>
    {
        private readonly RootContext _rootContext;

        public ChecklistItemChangeStateCommandHandler(RootContext rootContext)
        {
            _rootContext = rootContext;
        }

        public async Task Handle(ChecklistItemChangeStateCommand notification)
        {
            var item = _rootContext.ChecklistItemsForProject.Single(x => x.Id == notification.ItemId);
            item.Status = notification.State ? ChecklistItemStatus.Completed : ChecklistItemStatus.NotCompleted;
            await _rootContext.SaveChangesAsync();
        }
    }
}
