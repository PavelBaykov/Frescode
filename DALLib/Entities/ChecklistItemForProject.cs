using System;
using System.Collections.Generic;

namespace DALLib.Entities
{
    public class ChecklistItemForProject : ChecklistItem
    {
        public ChecklistForProject Checklist { get; set; }
        public ChecklistItemTemplate ItemTemplate { get; set; }
    }
    
}