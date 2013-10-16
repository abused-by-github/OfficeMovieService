namespace Svitla.MovieService.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTmdbMovie : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                EXEC sp_rename
                    @objname = 'dbo.Movie.ImageUrl',
                    @newname = 'CustomImageUrl',
                    @objtype = 'COLUMN'
            ");

            CreateTable(
                "dbo.TmdbMovie",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TmdbId = c.Long(nullable: false),
                        PosterPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Movie", "TmdbMovieId", c => c.Long());
            AddForeignKey("dbo.Movie", "TmdbMovieId", "dbo.TmdbMovie", "Id");
            CreateIndex("dbo.Movie", "TmdbMovieId");
        }
        
        public override void Down()
        {
            Sql(@"
                EXEC sp_rename
                    @objname = 'dbo.Movie.CustomImageUrl',
                    @newname = 'ImageUrl',
                    @objtype = 'COLUMN'
            ");

            DropIndex("dbo.Movie", new[] { "TmdbMovieId" });
            DropForeignKey("dbo.Movie", "TmdbMovieId", "dbo.TmdbMovie");
            DropColumn("dbo.Movie", "TmdbMovieId");
            DropTable("dbo.TmdbMovie");
        }
    }
}
