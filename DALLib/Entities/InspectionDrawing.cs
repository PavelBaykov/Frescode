using System;

namespace DALLib.Entities
{
    public class InspectionDrawing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; } /*Под вопросом*/
        public DateTime DateCreated { get; set; }/*Под вопросом*/
        public User CreatedBy { get; set; }/*Под вопросом*/
        public InspectionDrawingTemplate InspectionDrawingTemplate { get; set; }
        public int InspectionDrawingDataId { get; set; }
        public Structure Structure { get; set; }
    }

    public class InspectionDrawingTemplate
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public byte[] Data { get; set; }
    }
}