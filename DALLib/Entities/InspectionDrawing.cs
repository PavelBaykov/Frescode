using System;
using System.Collections.Generic;

namespace DALLib.Entities
{
    public class InspectionDrawing
    {
        public int Id { get; set; }
        //public string Name { get; set; }
        /*public int Size { get; set; }*/ /*Под вопросом*/
        /*public DateTime DateCreated { get; set; }*//*Под вопросом*/
        /*public User CreatedBy { get; set; }*//*Под вопросом*/
        public InspectionDrawingTemplate InspectionDrawingTemplate { get; set; }
        //public int InspectionDrawingDataId { get; set; }
        public ICollection<ChecklistForInspectionDrawing> Checklists { get; set; }
        public Structure Structure { get; set; }
        public ICollection<DefectionSpot> DefectionSpots { get; set; }
    }

    public class InspectionDrawingTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}