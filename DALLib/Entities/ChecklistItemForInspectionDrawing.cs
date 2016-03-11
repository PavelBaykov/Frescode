using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Entities
{
    public class ChecklistItemForInspectionDrawing : ChecklistItem
    {
        public ChecklistForInspectionDrawing Checklist { get; set; }
        public ChecklistItemTemplate ItemTemplate { get; set; }
    }
}
