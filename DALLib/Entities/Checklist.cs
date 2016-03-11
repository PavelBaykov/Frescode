using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Entities
{
    public class Checklist
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public User CreatedBy { get; set; }        
        public DateTime DateOfLastChange { get; set; }
        public User ChangedBy { get; set; }        
    }
}
