using System.Data.Entity;
using Svitla.MovieService.Core.Entities.EmailQueue;
using Svitla.MovieService.DataAccessApi;

namespace Svitla.MovieService.DataAccess
{
    public class EmailQueueRepository : BaseRepository<Email>, IEmailQueueRepository
    {
        public EmailQueueRepository(DataContext context) : base(context) { }

        protected override DbSet<Email> Set
        {
            get { return Context.EmailQueue; }
        }
    }
}
