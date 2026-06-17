namespace Elegant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWarehouseOutputDocuments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WarehouseOutputItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        WarehouseOutputId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.WarehouseOutputs", t => t.WarehouseOutputId, cascadeDelete: true)
                .Index(t => t.WarehouseOutputId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.WarehouseOutputs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OutputDate = c.DateTime(nullable: false),
                        Destination = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseOutputItems", "WarehouseOutputId", "dbo.WarehouseOutputs");
            DropForeignKey("dbo.WarehouseOutputItems", "ProductId", "dbo.Products");
            DropIndex("dbo.WarehouseOutputItems", new[] { "ProductId" });
            DropIndex("dbo.WarehouseOutputItems", new[] { "WarehouseOutputId" });
            DropTable("dbo.WarehouseOutputs");
            DropTable("dbo.WarehouseOutputItems");
        }
    }
}
