namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseUserIdLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("PostModels", "UserId", c => c.String(maxLength: 128, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("PostModels", "UserId", c => c.String(maxLength: 32, storeType: "nvarchar"));
        }
    }
}
