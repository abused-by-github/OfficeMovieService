using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.DomainApi;

namespace Svitla.MovieService.Domain.Facades
{
    public class UserFacade : BaseFacade, IUserFacade
    {
        private readonly IUserRepository users;

        public UserFacade(IUnitOfWork unitOfWork, IUserRepository userRepository)
            : base(unitOfWork)
        {
            users = userRepository;
        }

        public User GetByEmail(string email)
        {
            return users.One(q => q.FirstOrDefault(u => u.Name == email));
        }
        
        public void Save(User user)
        {
            var existedUser = GetByEmail(user.Name);
            if (existedUser != null)
            {
                user.Id = existedUser.Id;
            }
            users[user.Id] = user;
            UnitOfWork.Commit();
        }
    }
}
