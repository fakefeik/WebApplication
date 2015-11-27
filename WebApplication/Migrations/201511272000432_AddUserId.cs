namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("PostModels", "User_Id", "AspNetUsers");
            DropIndex("PostModels", new[] { "User_Id" });
            AddColumn("PostModels", "UserId", c => c.String(maxLength: 32, storeType: "nvarchar"));
            DropColumn("PostModels", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("PostModels", "User_Id", c => c.String(maxLength: 128, storeType: "nvarchar"));
            DropColumn("PostModels", "UserId");
            CreateIndex("PostModels", "User_Id");
            AddForeignKey("PostModels", "User_Id", "AspNetUsers", "Id");
        }
    }
}
