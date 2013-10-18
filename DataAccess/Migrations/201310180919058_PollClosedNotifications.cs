namespace Svitla.MovieService.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PollClosedNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vote", "HasNotificationBeenSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Poll", "HaveNotificationsBeenSent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Poll", "HaveNotificationsBeenSent");
            DropColumn("dbo.Vote", "HasNotificationBeenSent");
        }
    }
}
