namespace Svitla.MovieService.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserInvitedBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "InvitedBy_Id", c => c.Long());
            AddForeignKey("dbo.User", "InvitedBy_Id", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "InvitedBy_Id", "dbo.User");
            DropColumn("dbo.User", "InvitedBy_Id");
        }
    }
}
