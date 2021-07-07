namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initil : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dokuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        FileName = c.String(),
                        FileContent = c.Binary(),
                        InspektimId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspektims", t => t.InspektimId, cascadeDelete: true)
                .Index(t => t.InspektimId);
            
            CreateTable(
                "dbo.Inspektims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Emri = c.String(nullable: false),
                        Data = c.DateTime(nullable: false),
                        SubjektId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjekts", t => t.SubjektId, cascadeDelete: true)
                .Index(t => t.SubjektId);
            
            CreateTable(
                "dbo.Subjekts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Emri = c.String(nullable: false),
                        Data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inspektims", "SubjektId", "dbo.Subjekts");
            DropForeignKey("dbo.Dokuments", "InspektimId", "dbo.Inspektims");
            DropIndex("dbo.Inspektims", new[] { "SubjektId" });
            DropIndex("dbo.Dokuments", new[] { "InspektimId" });
            DropTable("dbo.Subjekts");
            DropTable("dbo.Inspektims");
            DropTable("dbo.Dokuments");
        }
    }
}
