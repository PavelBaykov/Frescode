using MediatR;

namespace Frescode.BL.Commands
{
    public class AddDefectSpotCommand : IAsyncNotification
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int ChecklistItemId { get; set; }

        public AddDefectSpotCommand(int id, string description, int orderNumber, double x, double y, int checklistItemId)
        {
            Id = id;
            Description = description;
            OrderNumber = orderNumber;
            ChecklistItemId = checklistItemId;
            X = x;
            Y = y;
        }
    }
}
