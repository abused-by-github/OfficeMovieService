using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.DataObjects;
using Svitla.MovieService.DomainApi;

namespace Svitla.MovieService.Domain.Facades
{
    public class UserFacade : BaseFacade, IUserFacade
    {
        private readonly IUserRepository users;

        public UserFacade(DomainContext domainContext, IUnitOfWork unitOfWork, IUserRepository userRepository)
            : base(unitOfWork, domainContext)
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
