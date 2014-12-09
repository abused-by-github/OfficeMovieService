using System.Data.Entity;
using System.Linq;
using MovieService.Core.Entities;
using MovieService.DataAccessApi;

namespace MovieService.DataAccess
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
