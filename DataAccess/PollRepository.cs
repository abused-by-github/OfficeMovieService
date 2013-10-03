using System.Data.Entity;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;

namespace Svitla.MovieService.DataAccess
{
    public class PollRepository : BaseRepository<Poll>, IPollRepository
    {
        public PollRepository(DataContext context) : base(context) { }

        protected override DbSet<Poll> Set
        {
            get { return Context.Polls; }
        }
    }
}
