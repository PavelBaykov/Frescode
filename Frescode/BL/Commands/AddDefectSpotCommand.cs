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
        public int InspectionDrawingId { get; set; }

        public AddDefectSpotCommand(int id, string description, int orderNumber, double x, double y, int inspectionDrawingId)
        {
            Id = id;
            Description = description;
            OrderNumber = orderNumber;
            InspectionDrawingId = inspectionDrawingId;
            X = x;
            Y = y;
        }
    }
}
