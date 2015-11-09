using System;
using System.Collections.Generic;

namespace Frescode.DAL.Entities
{
    public class Checklist
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public User CreatedBy { get; set; }
        public ICollection<ChecklistItem> Items { get; set; }
        public ChecklistTemplate ChecklistTemplate { get; set; }
        public DateTime DateOfLastChange { get; set; }
        public User ChangedBy { get; set; }
        public Project Project { get; set; }

    }
}