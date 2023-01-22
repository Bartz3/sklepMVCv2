namespace sklepMVCv2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userIDtoString : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Orders", new[] { "User_Id" });
            DropIndex("dbo.Complaints", new[] { "User_Id" });
            DropColumn("dbo.Orders", "UserID");
            DropColumn("dbo.Complaints", "UserID");
            RenameColumn(table: "dbo.Orders", name: "User_Id", newName: "UserID");
            RenameColumn(table: "dbo.Complaints", name: "User_Id", newName: "UserID");
            AlterColumn("dbo.Orders", "UserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Complaints", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "UserID");
            CreateIndex("dbo.Complaints", "UserID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Complaints", new[] { "UserID" });
            DropIndex("dbo.Orders", new[] { "UserID" });
            AlterColumn("dbo.Complaints", "UserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "UserID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Complaints", name: "UserID", newName: "User_Id");
            RenameColumn(table: "dbo.Orders", name: "UserID", newName: "User_Id");
            AddColumn("dbo.Complaints", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Complaints", "User_Id");
            CreateIndex("dbo.Orders", "User_Id");
        }
    }
}
