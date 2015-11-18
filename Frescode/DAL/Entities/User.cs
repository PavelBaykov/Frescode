using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Frescode.DAL.Entities
{
    public class User : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime DateCreated { get; set; }
        public User CreatedBy { get; set; }
        public object LastChangedItem { get; set; }
        public ICollection<Project> Projects { get; set; } 
        public Customer Customer { get; set; }
        public ICollection<Project> ProjectsOwned { get; set; }
    }
}