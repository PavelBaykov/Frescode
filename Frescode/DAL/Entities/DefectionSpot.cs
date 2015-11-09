using System;
using System.Collections.Generic;

namespace Frescode.DAL.Entities
{
    public class DefectionSpot
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int OrderNumber { get; set; }
        public ICollection<Picture> AttachedPictures { get; set; } 
        public DateTime DateCreated { get; set; }
        public ChecklistItem ChecklistItem { get; set; }
    }
}