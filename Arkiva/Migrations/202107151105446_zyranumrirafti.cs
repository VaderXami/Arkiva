namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zyranumrirafti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dokuments", "Zyra", c => c.String(nullable: false));
            AddColumn("dbo.Dokuments", "NrKutis", c => c.Int(nullable: false));
            AddColumn("dbo.Dokuments", "Rafti", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dokuments", "Rafti");
            DropColumn("dbo.Dokuments", "NrKutis");
            DropColumn("dbo.Dokuments", "Zyra");
        }
    }
}
