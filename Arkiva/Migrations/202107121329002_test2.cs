namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LlojiDokumentits", "Emri", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.Inspektims", "Emri", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.Subjekts", "Emri", c => c.String(nullable: false, maxLength: 35));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Subjekts", "Emri", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Inspektims", "Emri", c => c.String(nullable: false));
            AlterColumn("dbo.LlojiDokumentits", "Emri", c => c.String(nullable: false));
        }
    }
}
