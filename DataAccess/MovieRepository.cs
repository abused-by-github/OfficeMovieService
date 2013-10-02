using System.Data.Entity;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;

namespace Svitla.MovieService.DataAccess
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(string connectionString) : base(connectionString) { }

        protected override DbSet<Movie> Set
        {
            get { return Context.Movies; }
        }
    }
}
