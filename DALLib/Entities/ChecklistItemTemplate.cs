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

        //Далее- под вопросом
        public InspectionDrawing InspectionDrawing { get; set; }
        public ICollection<ChecklistItem> Descendants { get; set; } 
    }
}