using System.Data.Entity;
using MovieService.Core.Entities;
using MovieService.DataAccessApi;

namespace MovieService.DataAccess
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { }

        protected override DbSet<User> Set
        {
            get { return Context.Users; }
        }
    }
}
