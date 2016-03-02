using System.Collections.Generic;

namespace Frescode.DAL1.Entities
{
    public class ChecklistTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ChecklistItemTemplate> Items { get; set; }
        public ICollection<Checklist> Descendants { get; set; }
    }
}