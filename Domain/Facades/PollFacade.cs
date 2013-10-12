using System;
using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.DataObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.Exceptions;

namespace Svitla.MovieService.Domain.Facades
{
    public class PollFacade : BaseFacade, IPollFacade
    {
        private readonly IPollRepository polls;

        public PollFacade(DomainContext domainContext, IUnitOfWork unitOfWork, IPollRepository pollRepository)
            : base(unitOfWork, domainContext)
        {
            polls = pollRepository;
        }

        public virtual Poll GetCurrent()
        {
            return polls.One(q => q.SingleOrDefault(p => p.IsActive && !p.ViewDate.HasValue || p.ViewDate > DateTime.Now));
        }

        public virtual void CancelCurrent()
        {
            var current = GetCurrent();
            current.IsActive = false;

            UnitOfWork.Commit();
        }

        public virtual void Save(Poll poll)
        {
            var currentPoll = GetCurrent();
            if (currentPoll != null)
            {
                if (currentPoll != poll)
                {
                    throw new SinglePollAllowedException();
                }
                if (currentPoll.Owner != DomainContext.CurrentUser)
                {
                    throw new AccessDeniedException();
                }
            }

            poll.Owner = DomainContext.CurrentUser;

            poll.Validate();

            polls[poll.Id] = poll;

            UnitOfWork.Commit();
        }

        public virtual void Vote(User user, Movie movie, bool isSelected)
        {
            var poll = GetCurrent();
            var vote = new Vote { Movie = movie, Poll = poll, User = user };
            if (isSelected)
            {
                user.Votes.Add(vote);
            }
            else
            {
                user.Votes.Remove(vote);
            }
            UnitOfWork.Commit();
        }
    }
}
