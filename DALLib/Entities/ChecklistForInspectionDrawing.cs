using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Entities
{
    public class ChecklistForInspectionDrawing : Checklist
    {
        public InspectionDrawing InspectionDrawing { get; set; }
        public ICollection<ChecklistItemForInspectionDrawing> Items { get; set; }
        public ChecklistTemplate ChecklistTemplate { get; set; }
    }
}
