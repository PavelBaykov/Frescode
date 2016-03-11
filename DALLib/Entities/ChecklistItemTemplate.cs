using System.Collections.Generic;

namespace DALLib.Entities
{
    public class ChecklistItemTemplate
    {
        public int Id { get; set; }
        public ChecklistTemplate Checklist { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public ICollection<ChecklistItemForProject> DescendantsForProject { get; set; }
        public ICollection<ChecklistItemForInspectionDrawing> DescendantsForInspectionDrawing { get; set; }
    }
}