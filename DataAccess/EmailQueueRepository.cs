using System.Data.Entity;
using MovieService.Core.Entities.EmailQueue;
using MovieService.DataAccessApi;

namespace MovieService.DataAccess
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
