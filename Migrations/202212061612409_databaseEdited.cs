namespace sklepMVCv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class databaseEdited : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "AddressID", "dbo.Addresses");
            DropForeignKey("dbo.CategoryProducts1", "Category_CategoryID", "dbo.Categories");
            DropForeignKey("dbo.CategoryProducts1", "Product_ProductID", "dbo.Products");
            DropIndex("dbo.AspNetUsers", new[] { "AddressID" });
            DropIndex("dbo.CategoryProducts1", new[] { "Category_CategoryID" });
            DropIndex("dbo.CategoryProducts1", new[] { "Product_ProductID" });
            AddColumn("dbo.AspNetUsers", "City", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "Street", c => c.String(maxLength: 100));
            AddColumn("dbo.AspNetUsers", "HouseNumber", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "ZipCode", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.AspNetUsers", "AddressID");
            DropTable("dbo.Addresses");
            DropTable("dbo.CategoryProducts1");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CategoryProducts1",
                c => new
                    {
                        Category_CategoryID = c.Int(nullable: false),
                        Product_ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_CategoryID, t.Product_ProductID });
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressID = c.Int(nullable: false, identity: true),
                        City = c.String(nullable: false, maxLength: 100),
                        Street = c.String(nullable: false, maxLength: 100),
                        HouseNumber = c.Int(),
                        ZipCode = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.AddressID);
            
            AddColumn("dbo.AspNetUsers", "AddressID", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "ZipCode");
            DropColumn("dbo.AspNetUsers", "HouseNumber");
            DropColumn("dbo.AspNetUsers", "Street");
            DropColumn("dbo.AspNetUsers", "City");
            CreateIndex("dbo.CategoryProducts1", "Product_ProductID");
            CreateIndex("dbo.CategoryProducts1", "Category_CategoryID");
            CreateIndex("dbo.AspNetUsers", "AddressID");
            AddForeignKey("dbo.CategoryProducts1", "Product_ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
            AddForeignKey("dbo.CategoryProducts1", "Category_CategoryID", "dbo.Categories", "CategoryID", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "AddressID", "dbo.Addresses", "AddressID", cascadeDelete: true);
        }
    }
}
