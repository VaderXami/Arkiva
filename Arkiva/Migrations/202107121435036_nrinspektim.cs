namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nrinspektim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inspektims", "NrInspektimit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inspektims", "NrInspektimit");
        }
    }
}
