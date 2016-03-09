namespace DALLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChecklistItems", "ItemTemplate_Id", "dbo.ChecklistItemTemplates");
            DropIndex("dbo.ChecklistItems", new[] { "ItemTemplate_Id" });
            AlterColumn("dbo.ChecklistItems", "ItemTemplate_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.ChecklistItems", "ItemTemplate_Id");
            AddForeignKey("dbo.ChecklistItems", "ItemTemplate_Id", "dbo.ChecklistItemTemplates", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChecklistItems", "ItemTemplate_Id", "dbo.ChecklistItemTemplates");
            DropIndex("dbo.ChecklistItems", new[] { "ItemTemplate_Id" });
            AlterColumn("dbo.ChecklistItems", "ItemTemplate_Id", c => c.Int());
            CreateIndex("dbo.ChecklistItems", "ItemTemplate_Id");
            AddForeignKey("dbo.ChecklistItems", "ItemTemplate_Id", "dbo.ChecklistItemTemplates", "Id");
        }
    }
}
