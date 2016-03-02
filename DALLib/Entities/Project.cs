using System;
using System.Collections.Generic;

namespace DALLib.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
        public DateTime DateCreated { get; set; }
        public User CreatedBy { get; set; }
        public DateTime DateOfLastChange { get; set; }
        public User ChangedBy { get; set; }
        public ICollection<User> Members { get; set; }
        public ICollection<Checklist> Checklists { get; set; }
        public Customer Customer { get; set; }
        public ProjectStatus Status { get; set; }
    }

    public enum ProjectStatus
    {
        InProgress, Done, Reported
    }
}