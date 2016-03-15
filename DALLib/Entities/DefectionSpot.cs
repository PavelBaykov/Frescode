using System;
using System.Collections.Generic;

namespace DALLib.Entities
{
    public class DefectionSpot
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int OrderNumber { get; set; }
        public ICollection<Picture> AttachedPictures { get; set; } 
        public DateTime DateCreated { get; set; }
        public InspectionDrawing InspectionDrawing { get; set; }
    }
}