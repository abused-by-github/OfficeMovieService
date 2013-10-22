namespace Svitla.MovieService.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPollDiscussionDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Poll", "DiscussionDate", c => c.DateTimeOffset());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Poll", "DiscussionDate");
        }
    }
}
