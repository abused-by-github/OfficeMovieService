using System.Data.Entity.Infrastructure;

namespace Svitla.MovieService.DataAccess.Migrations
{
    //TODO: can't use IoC because of http://entityframework.codeplex.com/workitem/1131
    //So, connection string is hardcoded.
    public class MigrationsContextFactory : IDbContextFactory<DataContext>
    {
        public DataContext Create()
        {
            return new DataContext("ConnectionString");
        }
    }
}
