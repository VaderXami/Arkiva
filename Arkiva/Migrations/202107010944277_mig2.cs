namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Dokuments", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Dokuments", "FileName", c => c.String(nullable: false));
        }
    }
}
