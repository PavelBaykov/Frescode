using System;
using System.Collections.Generic;

namespace DALLib.Entities
{
    public class ChecklistForProject : Checklist
    {
        public Project Project { get; set; }
        public ICollection<ChecklistItemForProject> Items { get; set; }
        public ChecklistTemplate ChecklistTemplate { get; set; }
    }
}