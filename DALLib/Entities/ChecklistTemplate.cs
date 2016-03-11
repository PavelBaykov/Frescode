using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Entities
{
    public class ChecklistTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ChecklistItemTemplate> Items { get; set; }
        public ICollection<ChecklistForInspectionDrawing> DescendantsForInspectionDrawing { get; set; }
        public ICollection<ChecklistForProject> DescendantsForProject { get; set; }
    }
}
