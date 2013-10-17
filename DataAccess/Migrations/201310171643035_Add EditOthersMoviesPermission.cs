using Svitla.MovieService.Core.Entities.Security;

namespace Svitla.MovieService.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEditOthersMoviesPermission : DbMigration
    {
        public override void Up()
        {
            Sql(string.Format("insert into Permission(Code) values ({0})",
                (int) Permissions.EditOthersMovies));
        }
        
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
