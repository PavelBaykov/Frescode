namespace DALLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InspectionDrawings", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.InspectionDrawings", "InspectionDrawingTemplate_Id", "dbo.InspectionDrawingTemplates");
            DropForeignKey("dbo.InspectionDrawings", "Structure_Id", "dbo.Structures");
            DropForeignKey("dbo.ChecklistItemTemplates", "InspectionDrawing_Id", "dbo.InspectionDrawings");
            DropIndex("dbo.ChecklistItemTemplates", new[] { "InspectionDrawing_Id" });
            DropIndex("dbo.InspectionDrawings", new[] { "CreatedBy_Id" });
            DropIndex("dbo.InspectionDrawings", new[] { "InspectionDrawingTemplate_Id" });
            DropIndex("dbo.InspectionDrawings", new[] { "Structure_Id" });
            DropColumn("dbo.ChecklistItemTemplates", "InspectionDrawing_Id");
            DropTable("dbo.InspectionDrawings");
            DropTable("dbo.InspectionDrawingTemplates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InspectionDrawingTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        InspectionDrawingTemplate_Id = c.Int(),
                        Structure_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ChecklistItemTemplates", "InspectionDrawing_Id", c => c.Int());
            CreateIndex("dbo.InspectionDrawings", "Structure_Id");
            CreateIndex("dbo.InspectionDrawings", "InspectionDrawingTemplate_Id");
            CreateIndex("dbo.InspectionDrawings", "CreatedBy_Id");
            CreateIndex("dbo.ChecklistItemTemplates", "InspectionDrawing_Id");
            AddForeignKey("dbo.ChecklistItemTemplates", "InspectionDrawing_Id", "dbo.InspectionDrawings", "Id");
            AddForeignKey("dbo.InspectionDrawings", "Structure_Id", "dbo.Structures", "Id");
            AddForeignKey("dbo.InspectionDrawings", "InspectionDrawingTemplate_Id", "dbo.InspectionDrawingTemplates", "Id");
            AddForeignKey("dbo.InspectionDrawings", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
