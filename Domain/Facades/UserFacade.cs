using System;
using System.Linq;
using MovieService.Core.Entities;
using MovieService.Core.Entities.Security;
using MovieService.DataAccessApi;
using MovieService.Domain.DataObjects;
using MovieService.Domain.Security;
using MovieService.DomainApi;
using MovieService.DomainApi.Exceptions;
using MovieService.MailingApi;

namespace MovieService.Domain.Facades
{
    public class UserFacade : BaseFacade, IUserFacade
    {
        private readonly IUserRepository users;
        private readonly Func<IInviteEmail> inviteEmailFactory;

        //These domains are supported by oAuth. null - unrestricted.
        private readonly string[] domainsAllowedForInvintation = null;

        public string AllowedDomain { get; set; }

        public UserFacade(DomainContext domainContext, IUnitOfWork unitOfWork, IUserRepository userRepository, Func<IInviteEmail> inviteEmailFactory)
            : base(unitOfWork, domainContext)
        {
            users = userRepository;
            this.inviteEmailFactory = inviteEmailFactory;
        }

        public virtual User GetByEmail(string email)
        {
            return users.One(q => q.FirstOrDefault(u => u.Name == email));
        }
        
        public virtual void Save(User user)
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

        [Secure(Permissions.Invite)]
        public virtual void InviteFriend(User friend)
        {
            if (domainsAllowedForInvintation != null && domainsAllowedForInvintation.All(d => !friend.Name.EndsWith(d)))
            {
                var domains = domainsAllowedForInvintation.Aggregate("", (s, d) => s + ", " + d);
                throw new UserDomainDeniedException(domains.Substring(2));
            }
            var existingUser = GetByEmail(friend.Name);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException(friend.Name);
            }
            friend.InvitedBy = DomainContext.CurrentUser;
            users.Add(friend);
            UnitOfWork.Commit();

            var email = inviteEmailFactory();
            email.Bind(friend);
            email.Send(new [] { new EmailAddress(friend.Name) });
        }

        private bool IsDomainValid(string email)
        {
            return string.IsNullOrEmpty(AllowedDomain) || email.EndsWith(AllowedDomain);
        }
    }
}
