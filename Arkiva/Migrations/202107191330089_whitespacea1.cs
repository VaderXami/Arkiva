namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class whitespacea1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dokuments", "IsSelected", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dokuments", "Dokument_Id", c => c.Int());
            CreateIndex("dbo.Dokuments", "Dokument_Id");
            AddForeignKey("dbo.Dokuments", "Dokument_Id", "dbo.Dokuments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dokuments", "Dokument_Id", "dbo.Dokuments");
            DropIndex("dbo.Dokuments", new[] { "Dokument_Id" });
            DropColumn("dbo.Dokuments", "Dokument_Id");
            DropColumn("dbo.Dokuments", "IsSelected");
        }
    }
}
