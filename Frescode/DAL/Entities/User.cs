using System;
using System.Collections.Generic;

namespace Frescode.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime DateCreated { get; set; }
        public User CreatedBy { get; set; }
        public string Password { get; set; }
        public object LastChangedItem { get; set; }
        public ICollection<Project> Projects { get; set; } 
        public Customer Customer { get; set; }
        public ICollection<Project> ProjectsOwned { get; set; }
    }
}