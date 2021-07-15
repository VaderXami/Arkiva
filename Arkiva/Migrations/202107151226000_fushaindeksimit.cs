namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fushaindeksimit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dokuments", "Indeksimi", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dokuments", "Indeksimi");
        }
    }
}
