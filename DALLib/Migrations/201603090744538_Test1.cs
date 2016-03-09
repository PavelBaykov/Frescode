namespace DALLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Structures", "Id", "dbo.InspectionDrawings");
            DropForeignKey("dbo.Structures", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Structures", new[] { "Id" });
            DropIndex("dbo.Structures", new[] { "Project_Id" });
            DropPrimaryKey("dbo.Structures");
            AddColumn("dbo.InspectionDrawings", "Structure_Id", c => c.Int());
            AlterColumn("dbo.Structures", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Structures", "Project_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Structures", "Id");
            CreateIndex("dbo.InspectionDrawings", "Structure_Id");
            CreateIndex("dbo.Structures", "Project_Id");
            AddForeignKey("dbo.InspectionDrawings", "Structure_Id", "dbo.Structures", "Id");
            AddForeignKey("dbo.Structures", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Structures", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.InspectionDrawings", "Structure_Id", "dbo.Structures");
            DropIndex("dbo.Structures", new[] { "Project_Id" });
            DropIndex("dbo.InspectionDrawings", new[] { "Structure_Id" });
            DropPrimaryKey("dbo.Structures");
            AlterColumn("dbo.Structures", "Project_Id", c => c.Int());
            AlterColumn("dbo.Structures", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.InspectionDrawings", "Structure_Id");
            AddPrimaryKey("dbo.Structures", "Id");
            CreateIndex("dbo.Structures", "Project_Id");
            CreateIndex("dbo.Structures", "Id");
            AddForeignKey("dbo.Structures", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.Structures", "Id", "dbo.InspectionDrawings", "Id");
        }
    }
}
