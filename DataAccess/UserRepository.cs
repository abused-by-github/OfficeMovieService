using System.Data.Entity;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;

namespace Svitla.MovieService.DataAccess
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
