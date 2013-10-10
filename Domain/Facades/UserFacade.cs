using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.DataObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.Exceptions;

namespace Svitla.MovieService.Domain.Facades
{
    public class UserFacade : BaseFacade, IUserFacade
    {
        private readonly IUserRepository users;

        public string AllowedDomain { get; set; }

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
            else if (!IsDomainValid(user.Name))
            {
                throw new UserDomainDeniedException(AllowedDomain);
            }
            users[user.Id] = user;
            UnitOfWork.Commit();
        }

        public void InviteFriend(User friend)
        {
            var existingUser = GetByEmail(friend.Name);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException(friend.Name);
            }
            friend.InvitedBy = DomainContext.CurrentUser;
            users.Add(friend);
            UnitOfWork.Commit();
        }

        private bool IsDomainValid(string email)
        {
            return string.IsNullOrEmpty(AllowedDomain) || email.EndsWith(AllowedDomain);
        }
    }
}
