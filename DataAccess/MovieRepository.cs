using System.Data.Entity;
using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;

namespace Svitla.MovieService.DataAccess
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository (DataContext context) : base(context) { }

        protected override DbSet<Movie> Set
        {
            get { return Context.Movies; }
        }

        protected override IQueryable<Movie> Queryable
        {
            get { return Set.Include(m => m.User); }
        }
    }
}
