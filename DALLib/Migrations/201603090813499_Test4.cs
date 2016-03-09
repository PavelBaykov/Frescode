namespace DALLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChecklistItemTemplates", "Checklist_Id", "dbo.ChecklistTemplates");
            DropIndex("dbo.ChecklistItemTemplates", new[] { "Checklist_Id" });
            AlterColumn("dbo.ChecklistItemTemplates", "Checklist_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.ChecklistItemTemplates", "Checklist_Id");
            AddForeignKey("dbo.ChecklistItemTemplates", "Checklist_Id", "dbo.ChecklistTemplates", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChecklistItemTemplates", "Checklist_Id", "dbo.ChecklistTemplates");
            DropIndex("dbo.ChecklistItemTemplates", new[] { "Checklist_Id" });
            AlterColumn("dbo.ChecklistItemTemplates", "Checklist_Id", c => c.Int());
            CreateIndex("dbo.ChecklistItemTemplates", "Checklist_Id");
            AddForeignKey("dbo.ChecklistItemTemplates", "Checklist_Id", "dbo.ChecklistTemplates", "Id");
        }
    }
}
