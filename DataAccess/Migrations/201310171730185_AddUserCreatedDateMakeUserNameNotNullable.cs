namespace MovieService.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserCreatedDateMakeUserNameNotNullable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "CreatedDate", c => c.DateTimeOffset(nullable: false));
            Sql("update [user] set CreatedDate = getdate()");
            AlterColumn("dbo.User", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "Name", c => c.String());
            DropColumn("dbo.User", "CreatedDate");
        }
    }
}
