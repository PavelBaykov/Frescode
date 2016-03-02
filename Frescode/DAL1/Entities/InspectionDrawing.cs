using System;

namespace Frescode.DAL1.Entities
{
    public class InspectionDrawing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public DateTime DateCreated { get; set; }
        public User CreatedBy { get; set; }
        public InspectionDrawingData InspectionDrawingData { get; set; }
        public int InspectionDrawingDataId { get; set; }
    }

    public class InspectionDrawingData
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
    }
}