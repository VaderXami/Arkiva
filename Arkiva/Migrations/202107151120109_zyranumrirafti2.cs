namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zyranumrirafti2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Dokuments", "Zyra", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Dokuments", "Zyra", c => c.String());
        }
    }
}
