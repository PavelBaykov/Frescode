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
        
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistItemTemplate> ChecklistItemTemplates { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public DbSet<ChecklistTemplate> ChecklistTemplates { get; set; }
        //public DbSet<InspectionDrawing> InspectionDrawings { get; set; }
        //public DbSet<DefectionSpot> DefectionSpots { get; set; }
        //public DbSet<Picture> Pictures { get; set; }
        //public DbSet<PictureData> PicturesData { get; set; }
        //public DbSet<InspectionDrawingTemplate> InspectionDrawingTemplate { get; set; }



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

            modelBuilder.Entity<Checklist>()
                .HasMany(checklist => checklist.Items)
                .WithRequired(item => item.Checklist);

            modelBuilder.Entity<Checklist>()
                .HasRequired(checklist => checklist.ChecklistTemplate)
                .WithMany(template => template.Descendants)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Checklist>()
                .HasRequired(checklist => checklist.Project)
                .WithMany(project => project.Checklists);

            modelBuilder.Entity<ChecklistTemplate>()
                .HasMany(template => template.Descendants)
                .WithRequired(checklist => checklist.ChecklistTemplate);

            //modelBuilder.Entity<ChecklistTemplate>()
            //    .HasMany(template => template.Items)
            //    .WithRequired(item => item.Checklist);

            modelBuilder.Entity<ChecklistItemTemplate>()
                .HasRequired(item => item.Checklist)
                .WithMany(template => template.Items);

            modelBuilder.Entity<ChecklistItemTemplate>()
                .HasMany(itemTemplate => itemTemplate.Descendants)
                .WithRequired(item => item.ItemTemplate);

            //modelBuilder.Entity<ChecklistItem>()
            //    .HasMany(item => item.DefectionSpots)
            //    .WithRequired(spot => spot.ChecklistItem);

            modelBuilder.Entity<ChecklistItem>()
                .HasRequired(item => item.Checklist)
                .WithMany(checklist => checklist.Items);

            //modelBuilder.Entity<ChecklistItem>()
            //   .HasRequired(item => item.ItemTemplate)
            //   .WithMany(template => template.Descendants)
            //   .WillCascadeOnDelete(false);

            //modelBuilder.Entity<DefectionSpot>()
            //   .HasRequired(spot => spot.ChecklistItem)
            //   .WithMany(item => item.DefectionSpots);

            //modelBuilder.Entity<DefectionSpot>()
            //   .HasMany(spot => spot.AttachedPictures)
            //   .WithRequired(picture => picture.DefectionSpot);

            //modelBuilder.Entity<Picture>()
            //    .HasRequired(picture => picture.DefectionSpot)
            //    .WithMany(spot => spot.AttachedPictures);

            //modelBuilder.Entity<Picture>()
            //    .HasRequired(picture => picture.PictureData)
            //    .WithRequiredPrincipal()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<InspectionDrawing>()
            //    .HasRequired(inspectionDrawing => inspectionDrawing.InspectionDrawingData)
            //    .WithMany()
            //    .HasForeignKey(x => x.InspectionDrawingDataId);
            //modelBuilder.Entity<Structure>()
            //    .HasRequired(structure => structure.InspectionDrawing)
            //    .WithRequiredDependent(id => id.Structure);

            modelBuilder.Entity<Project>()
                .HasMany(proj => proj.Structures)
                .WithRequired(s => s.Project)
                .WillCascadeOnDelete(true);
        }
    }
}
