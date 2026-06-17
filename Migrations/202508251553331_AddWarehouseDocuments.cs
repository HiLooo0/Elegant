namespace Elegant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWarehouseDocuments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WarehouseEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntryDate = c.DateTime(nullable: false),
                        Source = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WarehouseEntryItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        WarehouseEntryId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.WarehouseEntries", t => t.WarehouseEntryId, cascadeDelete: true)
                .Index(t => t.WarehouseEntryId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseEntryItems", "WarehouseEntryId", "dbo.WarehouseEntries");
            DropForeignKey("dbo.WarehouseEntryItems", "ProductId", "dbo.Products");
            DropIndex("dbo.WarehouseEntryItems", new[] { "ProductId" });
            DropIndex("dbo.WarehouseEntryItems", new[] { "WarehouseEntryId" });
            DropTable("dbo.WarehouseEntryItems");
            DropTable("dbo.WarehouseEntries");
        }
    }
}
