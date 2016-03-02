using System;
using System.Collections.Generic;

namespace DALLib.Entities
{
    public class ChecklistItem
    {
        public int Id { get; set; }
        public ChecklistItemStatus Status { get; set; }
        public ICollection<DefectionSpot> DefectionSpots { get; set; }
        public DateTime DateOfLastChange { get; set; }
        public User ChangedBy { get; set; }
        public Checklist Checklist { get; set; }
        public ChecklistItemTemplate ItemTemplate { get; set; }
    }
}