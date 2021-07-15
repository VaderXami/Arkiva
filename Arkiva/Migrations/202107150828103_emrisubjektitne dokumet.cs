namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emrisubjektitnedokumet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dokuments", "EmriSubjektit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dokuments", "EmriSubjektit");
        }
    }
}
