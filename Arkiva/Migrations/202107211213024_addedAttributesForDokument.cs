namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedAttributesForDokument : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Dokuments", "Zyra", c => c.String(nullable: false, maxLength: 35));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Dokuments", "Zyra", c => c.String(nullable: false));
        }
    }
}
