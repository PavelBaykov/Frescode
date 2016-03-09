namespace DALLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InspectionDrawing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InspectionDrawings", "InspectionDrawingDataId", "dbo.InspectionDrawingDatas");
            DropIndex("dbo.InspectionDrawings", new[] { "InspectionDrawingDataId" });
            DropPrimaryKey("dbo.Structures");
            CreateTable(
                "dbo.InspectionDrawingTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.InspectionDrawings", "InspectionDrawingTemplate_Id", c => c.Int());
            AlterColumn("dbo.Structures", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Structures", "Id");
            CreateIndex("dbo.InspectionDrawings", "InspectionDrawingTemplate_Id");
            CreateIndex("dbo.Structures", "Id");
            AddForeignKey("dbo.InspectionDrawings", "InspectionDrawingTemplate_Id", "dbo.InspectionDrawingTemplates", "Id");
            AddForeignKey("dbo.Structures", "Id", "dbo.InspectionDrawings", "Id");
            DropTable("dbo.InspectionDrawingDatas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InspectionDrawingDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Structures", "Id", "dbo.InspectionDrawings");
            DropForeignKey("dbo.InspectionDrawings", "InspectionDrawingTemplate_Id", "dbo.InspectionDrawingTemplates");
            DropIndex("dbo.Structures", new[] { "Id" });
            DropIndex("dbo.InspectionDrawings", new[] { "InspectionDrawingTemplate_Id" });
            DropPrimaryKey("dbo.Structures");
            AlterColumn("dbo.Structures", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.InspectionDrawings", "InspectionDrawingTemplate_Id");
            DropTable("dbo.InspectionDrawingTemplates");
            AddPrimaryKey("dbo.Structures", "Id");
            CreateIndex("dbo.InspectionDrawings", "InspectionDrawingDataId");
            AddForeignKey("dbo.InspectionDrawings", "InspectionDrawingDataId", "dbo.InspectionDrawingDatas", "Id", cascadeDelete: true);
        }
    }
}
