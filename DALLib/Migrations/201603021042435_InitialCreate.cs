namespace DALLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChecklistItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        DateOfLastChange = c.DateTime(nullable: false),
                        ChangedBy_Id = c.String(maxLength: 128),
                        Checklist_Id = c.Int(nullable: false),
                        ItemTemplate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ChangedBy_Id)
                .ForeignKey("dbo.Checklists", t => t.Checklist_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChecklistItemTemplates", t => t.ItemTemplate_Id)
                .Index(t => t.ChangedBy_Id)
                .Index(t => t.Checklist_Id)
                .Index(t => t.ItemTemplate_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserRole = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        CreatedBy_Id = c.String(maxLength: 128),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateOfLastChange = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        ChangedBy_Id = c.String(maxLength: 128),
                        CreatedBy_Id = c.String(maxLength: 128),
                        Owner_Id = c.String(nullable: false, maxLength: 128),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ChangedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.ChangedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Checklists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        DateOfLastChange = c.DateTime(nullable: false),
                        ChangedBy_Id = c.String(maxLength: 128),
                        ChecklistTemplate_Id = c.Int(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ChangedBy_Id)
                .ForeignKey("dbo.ChecklistTemplates", t => t.ChecklistTemplate_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.ChangedBy_Id)
                .Index(t => t.ChecklistTemplate_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.ChecklistTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChecklistItemTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        OrderNumber = c.Int(nullable: false),
                        Checklist_Id = c.Int(nullable: false),
                        InspectionDrawing_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChecklistTemplates", t => t.Checklist_Id, cascadeDelete: true)
                .ForeignKey("dbo.InspectionDrawings", t => t.InspectionDrawing_Id)
                .Index(t => t.Checklist_Id)
                .Index(t => t.InspectionDrawing_Id);
            
            CreateTable(
                "dbo.InspectionDrawings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Size = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        InspectionDrawingDataId = c.Int(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.InspectionDrawingDatas", t => t.InspectionDrawingDataId, cascadeDelete: true)
                .Index(t => t.InspectionDrawingDataId)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.InspectionDrawingDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.DefectionSpots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        OrderNumber = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        ChecklistItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChecklistItems", t => t.ChecklistItem_Id, cascadeDelete: true)
                .Index(t => t.ChecklistItem_Id);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateCaptured = c.DateTime(nullable: false),
                        PictureDataId = c.Int(nullable: false),
                        DefectionSpot_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DefectionSpots", t => t.DefectionSpot_Id, cascadeDelete: true)
                .Index(t => t.DefectionSpot_Id);
            
            CreateTable(
                "dbo.PictureDatas",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Data = c.Binary(),
                        Picture_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.Picture_Id)
                .ForeignKey("dbo.Pictures", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Picture_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Project_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ChecklistItems", "ItemTemplate_Id", "dbo.ChecklistItemTemplates");
            DropForeignKey("dbo.DefectionSpots", "ChecklistItem_Id", "dbo.ChecklistItems");
            DropForeignKey("dbo.Pictures", "DefectionSpot_Id", "dbo.DefectionSpots");
            DropForeignKey("dbo.PictureDatas", "Id", "dbo.Pictures");
            DropForeignKey("dbo.PictureDatas", "Picture_Id", "dbo.Pictures");
            DropForeignKey("dbo.ChecklistItems", "Checklist_Id", "dbo.Checklists");
            DropForeignKey("dbo.ChecklistItems", "ChangedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.UserProjects", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Projects", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Projects", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Checklists", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Checklists", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Checklists", "ChecklistTemplate_Id", "dbo.ChecklistTemplates");
            DropForeignKey("dbo.ChecklistItemTemplates", "InspectionDrawing_Id", "dbo.InspectionDrawings");
            DropForeignKey("dbo.InspectionDrawings", "InspectionDrawingDataId", "dbo.InspectionDrawingDatas");
            DropForeignKey("dbo.InspectionDrawings", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ChecklistItemTemplates", "Checklist_Id", "dbo.ChecklistTemplates");
            DropForeignKey("dbo.Checklists", "ChangedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "ChangedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserProjects", new[] { "Project_Id" });
            DropIndex("dbo.UserProjects", new[] { "User_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PictureDatas", new[] { "Picture_Id" });
            DropIndex("dbo.PictureDatas", new[] { "Id" });
            DropIndex("dbo.Pictures", new[] { "DefectionSpot_Id" });
            DropIndex("dbo.DefectionSpots", new[] { "ChecklistItem_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.InspectionDrawings", new[] { "CreatedBy_Id" });
            DropIndex("dbo.InspectionDrawings", new[] { "InspectionDrawingDataId" });
            DropIndex("dbo.ChecklistItemTemplates", new[] { "InspectionDrawing_Id" });
            DropIndex("dbo.ChecklistItemTemplates", new[] { "Checklist_Id" });
            DropIndex("dbo.Checklists", new[] { "Project_Id" });
            DropIndex("dbo.Checklists", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Checklists", new[] { "ChecklistTemplate_Id" });
            DropIndex("dbo.Checklists", new[] { "ChangedBy_Id" });
            DropIndex("dbo.Projects", new[] { "Customer_Id" });
            DropIndex("dbo.Projects", new[] { "Owner_Id" });
            DropIndex("dbo.Projects", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Projects", new[] { "ChangedBy_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Customer_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "CreatedBy_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ChecklistItems", new[] { "ItemTemplate_Id" });
            DropIndex("dbo.ChecklistItems", new[] { "Checklist_Id" });
            DropIndex("dbo.ChecklistItems", new[] { "ChangedBy_Id" });
            DropTable("dbo.UserProjects");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PictureDatas");
            DropTable("dbo.Pictures");
            DropTable("dbo.DefectionSpots");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.InspectionDrawingDatas");
            DropTable("dbo.InspectionDrawings");
            DropTable("dbo.ChecklistItemTemplates");
            DropTable("dbo.ChecklistTemplates");
            DropTable("dbo.Checklists");
            DropTable("dbo.Projects");
            DropTable("dbo.Customers");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ChecklistItems");
        }
    }
}
