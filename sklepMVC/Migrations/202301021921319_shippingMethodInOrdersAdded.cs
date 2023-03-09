namespace sklepMVCv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shippingMethodInOrdersAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ShippingMethod", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ShippingMethod");
        }
    }
}
