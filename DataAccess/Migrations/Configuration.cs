using System.Data.Entity.Migrations;

namespace Svitla.MovieService.DataAccess.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
