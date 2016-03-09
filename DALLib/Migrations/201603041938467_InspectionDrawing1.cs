namespace DALLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InspectionDrawing1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Structures", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Structures", new[] { "Project_Id" });
            AlterColumn("dbo.Structures", "Project_Id", c => c.Int());
            CreateIndex("dbo.Structures", "Project_Id");
            AddForeignKey("dbo.Structures", "Project_Id", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Structures", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Structures", new[] { "Project_Id" });
            AlterColumn("dbo.Structures", "Project_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Structures", "Project_Id");
            AddForeignKey("dbo.Structures", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
