namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newvlues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dokuments", "EmriInspektuesit", c => c.String());
            AddColumn("dbo.Dokuments", "NrInspektuesit", c => c.Int(nullable: false));
            AddColumn("dbo.Dokuments", "EmriLlojitDokumentit", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dokuments", "EmriLlojitDokumentit");
            DropColumn("dbo.Dokuments", "NrInspektuesit");
            DropColumn("dbo.Dokuments", "EmriInspektuesit");
        }
    }
}
