using System.Data.Entity;
using DALLib.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DALLib
{
    public class RootContext : IdentityDbContext<User>
    {
        public RootContext() : base("DefaultConnection")
        {

        }

        public static RootContext Create()
        {
            return new RootContext();
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Structure> Structures { get; set; }
        
        public DbSet<ChecklistForProject> ChecklistsForProject { get; set; }
        public DbSet<ChecklistItemForProject> ChecklistItemsForProject { get; set; }
        public DbSet<ChecklistItemTemplate> ChecklistItemTemplates { get; set; }
        public DbSet<ChecklistTemplate> ChecklistTemplates { get; set; }

        public DbSet<ChecklistForInspectionDrawing> ChecklistsForInspectionDrawing { get; set; }
        public DbSet<ChecklistItemForInspectionDrawing> ChecklistItemsForInspectionDrawing { get; set; }
        public DbSet<InspectionDrawing> InspectionDrawings { get; set; }
        public DbSet<InspectionDrawingTemplate> InspectionDrawingTemplate { get; set; }
        public DbSet<DefectionSpot> DefectionSpots { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PictureData> PicturesData { get; set; }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                .HasRequired(project => project.Customer)
                .WithMany(customer => customer.Projects)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasRequired(project => project.Owner)
                .WithMany(user => user.ProjectsOwned)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasMany(project => project.Checklists)
                .WithRequired(checklist => checklist.Project);

            modelBuilder.Entity<User>()
                .HasOptional(user => user.Customer)
                .WithMany(customer => customer.Users)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Projects)
                .WithMany(project => project.Members);

            modelBuilder.Entity<User>()
                .HasMany(user => user.ProjectsOwned)
                .WithRequired(project => project.Owner);

            modelBuilder.Entity<Customer>()
                .HasMany(customer => customer.Projects)
                .WithRequired(project => project.Customer);

            modelBuilder.Entity<Customer>()
                .HasMany(customer => customer.Users)
                .WithOptional(user => user.Customer);

            modelBuilder.Entity<ChecklistForProject>()
                .HasMany(checklist => checklist.Items)
                .WithRequired(item => item.Checklist);

            modelBuilder.Entity<ChecklistForProject>()
                .HasRequired(checklist => checklist.ChecklistTemplate)
                .WithMany(template => template.DescendantsForProject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ChecklistForProject>()
                .HasRequired(checklist => checklist.Project)
                .WithMany(project => project.Checklists);

            modelBuilder.Entity<ChecklistItemForProject>()
                .HasRequired(item => item.Checklist)
                .WithMany(checklist => checklist.Items);

            modelBuilder.Entity<ChecklistItemForProject>()
               .HasRequired(item => item.ItemTemplate)
               .WithMany(template => template.DescendantsForProject)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<ChecklistTemplate>()
                .HasMany(template => template.DescendantsForProject)
                .WithRequired(checklist => checklist.ChecklistTemplate);

            modelBuilder.Entity<ChecklistTemplate>()
                .HasMany(template => template.Items)
                .WithRequired(item => item.Checklist);

            modelBuilder.Entity<ChecklistItemTemplate>()
                .HasRequired(item => item.Checklist)
                .WithMany(template => template.Items);

            modelBuilder.Entity<ChecklistItemTemplate>()
                .HasMany(itemTemplate => itemTemplate.DescendantsForProject)
                .WithRequired(item => item.ItemTemplate);

            modelBuilder.Entity<Project>()
                .HasMany(proj => proj.Structures)
                .WithRequired(s => s.Project)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Structure>()
                .HasOptional(structure => structure.InspectionDrawing)
                .WithRequired(inspDrawing => inspDrawing.Structure)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<InspectionDrawing>()
                .HasMany(drawing => drawing.DefectionSpots)
                .WithRequired(spot => spot.InspectionDrawing)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<InspectionDrawing>()
                .HasMany(drawing => drawing.Checklists)
                .WithRequired(list => list.InspectionDrawing)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<InspectionDrawing>()
                .HasMany(drawing => drawing.DefectionSpots)
                .WithRequired(spot => spot.InspectionDrawing)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<DefectionSpot>()
               .HasMany(spot => spot.AttachedPictures)
               .WithRequired(picture => picture.DefectionSpot);

            modelBuilder.Entity<Picture>()
                .HasRequired(picture => picture.DefectionSpot)
                .WithMany(spot => spot.AttachedPictures);

            modelBuilder.Entity<Picture>()
                .HasRequired(picture => picture.PictureData)
                .WithRequiredPrincipal()
                .WillCascadeOnDelete(true);

            

        }
    }
}
