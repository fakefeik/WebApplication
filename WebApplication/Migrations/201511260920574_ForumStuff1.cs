namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumStuff1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardModels", "Name", c => c.String(maxLength: 32, storeType: "nvarchar"));
            AddColumn("dbo.PostModels", "Topic", c => c.String(maxLength: 32, storeType: "nvarchar"));
            AddColumn("dbo.PostModels", "Text", c => c.String(maxLength: 200, storeType: "nvarchar"));
            AddColumn("dbo.PostModels", "ThreadId", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.PostModels", "User_Id", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.ThreadModels", "Topic", c => c.String(maxLength: 32, storeType: "nvarchar"));
            AddColumn("dbo.ThreadModels", "Text", c => c.String(maxLength: 200, storeType: "nvarchar"));
            AddColumn("dbo.ThreadModels", "BoardId", c => c.String(maxLength: 64, storeType: "nvarchar"));
            CreateIndex("dbo.PostModels", "User_Id");
            AddForeignKey("dbo.PostModels", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostModels", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PostModels", new[] { "User_Id" });
            DropColumn("dbo.ThreadModels", "BoardId");
            DropColumn("dbo.ThreadModels", "Text");
            DropColumn("dbo.ThreadModels", "Topic");
            DropColumn("dbo.PostModels", "User_Id");
            DropColumn("dbo.PostModels", "ThreadId");
            DropColumn("dbo.PostModels", "Text");
            DropColumn("dbo.PostModels", "Topic");
            DropColumn("dbo.BoardModels", "Name");
        }
    }
}
