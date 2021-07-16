namespace Arkiva.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dokuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmriSubjektit = c.String(),
                        EmriInspektuesit = c.String(),
                        NrInspektuesit = c.Int(nullable: false),
                        EmriLlojitDokumentit = c.String(),
                        Data = c.DateTime(nullable: false),
                        FileName = c.String(),
                        FileContent = c.Binary(),
                        Zyra = c.String(nullable: false),
                        NrKutis = c.Int(nullable: false),
                        Rafti = c.Int(nullable: false),
                        Indeksimi = c.Int(nullable: false),
                        LlojiDokumentitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LlojiDokumentits", t => t.LlojiDokumentitId, cascadeDelete: true)
                .Index(t => t.LlojiDokumentitId);
            
            CreateTable(
                "dbo.LlojiDokumentits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Emri = c.String(nullable: false, maxLength: 35),
                        Data = c.DateTime(nullable: false),
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
                        Emri = c.String(nullable: false, maxLength: 35),
                        NrInspektimit = c.Int(nullable: false),
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
                        Emri = c.String(nullable: false, maxLength: 35),
                        Data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inspektims", "SubjektId", "dbo.Subjekts");
            DropForeignKey("dbo.LlojiDokumentits", "InspektimId", "dbo.Inspektims");
            DropForeignKey("dbo.Dokuments", "LlojiDokumentitId", "dbo.LlojiDokumentits");
            DropIndex("dbo.Inspektims", new[] { "SubjektId" });
            DropIndex("dbo.LlojiDokumentits", new[] { "InspektimId" });
            DropIndex("dbo.Dokuments", new[] { "LlojiDokumentitId" });
            DropTable("dbo.Subjekts");
            DropTable("dbo.Inspektims");
            DropTable("dbo.LlojiDokumentits");
            DropTable("dbo.Dokuments");
        }
    }
}
