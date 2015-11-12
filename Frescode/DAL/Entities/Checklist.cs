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

        public void ChecklistInit()
        {
            foreach (var tempItem in ChecklistTemplate.Items)
            {
                var checkListItem_tmp = new ChecklistItem
                {
                    Checklist = this,
                    ItemTemplate = tempItem,
                    Status = ChecklistItemStatus.NotCompleted,
                    DateOfLastChange = DateTime.UtcNow,
                    ChangedBy = Project.ChangedBy
                };
                Items.Add(checkListItem_tmp);
                tempItem.Descendants.Add(checkListItem_tmp);
                
            }
        }

    }
}