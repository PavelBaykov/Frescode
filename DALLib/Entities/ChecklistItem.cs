using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Entities
{
    public class ChecklistItem
    {
        public int Id { get; set; }
        public ChecklistItemStatus Status { get; set; }
        public DateTime DateOfLastChange { get; set; }
        public User ChangedBy { get; set; }
        
        
    }
    public enum ChecklistItemStatus
    {
        NotCompleted, Completed
    }
}
