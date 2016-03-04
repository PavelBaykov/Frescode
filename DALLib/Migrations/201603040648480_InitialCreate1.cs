namespace DALLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Structures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.Int(nullable: false),
                        Name = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Structures", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Structures", new[] { "Project_Id" });
            DropTable("dbo.Structures");
        }
    }
}
