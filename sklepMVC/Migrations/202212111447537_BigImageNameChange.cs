namespace sklepMVCv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BigImageNameChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Image", c => c.Binary());
            DropColumn("dbo.Products", "BigImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "BigImage", c => c.Binary());
            DropColumn("dbo.Products", "Image");
        }
    }
}
