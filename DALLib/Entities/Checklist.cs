using System;
using System.Collections.Generic;

namespace DALLib.Entities
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

        public void ChecklistInit(RootContext rootContext)
        {
            foreach (var tempItem in ChecklistTemplate.Items)
            {
                var checkListItemTmp = new ChecklistItem
                {
                    Checklist = this,
                    ItemTemplate = tempItem,
                    Status = ChecklistItemStatus.NotCompleted,
                    DateOfLastChange = DateTime.UtcNow,
                    ChangedBy = Project.ChangedBy
                };
                Items.Add(checkListItemTmp);
                tempItem.Descendants.Add(checkListItemTmp);
                rootContext.ChecklistItems.Add(checkListItemTmp);
            }
        }

    }
}