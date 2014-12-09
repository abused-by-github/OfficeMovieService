namespace MovieService.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vote",
                c => new
                    {
                        MovieId = c.Long(nullable: false),
                        PollId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieId, t.PollId, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Poll", t => t.PollId, cascadeDelete: true)
                .ForeignKey("dbo.Movie", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PollId)
                .Index(t => t.MovieId);
            
            CreateTable(
                "dbo.Poll",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false),
                        ExpirationDate = c.DateTimeOffset(),
                        ViewDate = c.DateTimeOffset(),
                        IsActive = c.Boolean(nullable: false),
                        Owner_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Movie",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        ImageUrl = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false),
                        ModifiedDate = c.DateTimeOffset(nullable: false),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Movie", new[] { "User_Id" });
            DropIndex("dbo.Poll", new[] { "Owner_Id" });
            DropIndex("dbo.Vote", new[] { "MovieId" });
            DropIndex("dbo.Vote", new[] { "PollId" });
            DropIndex("dbo.Vote", new[] { "UserId" });
            DropForeignKey("dbo.Movie", "User_Id", "dbo.User");
            DropForeignKey("dbo.Poll", "Owner_Id", "dbo.User");
            DropForeignKey("dbo.Vote", "MovieId", "dbo.Movie");
            DropForeignKey("dbo.Vote", "PollId", "dbo.Poll");
            DropForeignKey("dbo.Vote", "UserId", "dbo.User");
            DropTable("dbo.Movie");
            DropTable("dbo.Poll");
            DropTable("dbo.Vote");
            DropTable("dbo.User");
        }
    }
}
