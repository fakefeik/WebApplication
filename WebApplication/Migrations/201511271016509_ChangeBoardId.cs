namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBoardId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.BoardModels");
            AddColumn("dbo.BoardModels", "ShortName", c => c.String(nullable: false, maxLength: 4, storeType: "nvarchar"));
            AlterColumn("dbo.PostModels", "ThreadId", c => c.Int(nullable: false));
            AlterColumn("dbo.ThreadModels", "BoardId", c => c.String(unicode: false));
            AddPrimaryKey("dbo.BoardModels", "ShortName");
            DropColumn("dbo.BoardModels", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BoardModels", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.BoardModels");
            AlterColumn("dbo.ThreadModels", "BoardId", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AlterColumn("dbo.PostModels", "ThreadId", c => c.String(maxLength: 64, storeType: "nvarchar"));
            DropColumn("dbo.BoardModels", "ShortName");
            AddPrimaryKey("dbo.BoardModels", "Id");
        }
    }
}
