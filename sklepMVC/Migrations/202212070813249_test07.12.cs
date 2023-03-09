namespace sklepMVCv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test0712 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "ZipCode", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "ZipCode", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
