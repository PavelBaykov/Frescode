using System.Collections.Generic;

namespace DALLib.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Project> Projects { get; set; }  
    }
}