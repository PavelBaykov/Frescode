namespace DALLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class structures : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Structures", "Path", c => c.String());
            AlterColumn("dbo.Structures", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Structures", "Name", c => c.Int(nullable: false));
            AlterColumn("dbo.Structures", "Path", c => c.Int(nullable: false));
        }
    }
}
