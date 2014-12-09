using System.Data.Entity;
using MovieService.Core.Entities;
using MovieService.DataAccessApi;

namespace MovieService.DataAccess
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
