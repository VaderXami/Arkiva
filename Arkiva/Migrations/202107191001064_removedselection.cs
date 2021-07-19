namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedselection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dokuments", "Dokument_Id", "dbo.Dokuments");
            DropIndex("dbo.Dokuments", new[] { "Dokument_Id" });
            DropColumn("dbo.Dokuments", "IsSelected");
            DropColumn("dbo.Dokuments", "Dokument_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dokuments", "Dokument_Id", c => c.Int());
            AddColumn("dbo.Dokuments", "IsSelected", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Dokuments", "Dokument_Id");
            AddForeignKey("dbo.Dokuments", "Dokument_Id", "dbo.Dokuments", "Id");
        }
    }
}
