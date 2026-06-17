namespace Elegant.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductShopFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "OldPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Products", "ImageUrl", c => c.String());
            AddColumn("dbo.Products", "Stock", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "IsFeatured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsFeatured");
            DropColumn("dbo.Products", "Stock");
            DropColumn("dbo.Products", "ImageUrl");
            DropColumn("dbo.Products", "OldPrice");
        }
    }
}
