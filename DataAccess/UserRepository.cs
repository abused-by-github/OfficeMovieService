using System.Data.Entity;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;

namespace Svitla.MovieService.DataAccess
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString) { }

        protected override DbSet<User> Set
        {
            get { return Context.Users; }
        }
    }
}
