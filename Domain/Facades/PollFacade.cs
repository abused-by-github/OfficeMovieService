using System;
using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.Exceptions;

namespace Svitla.MovieService.Domain.Facades
{
    public class PollFacade : BaseFacade, IPollFacade
    {
        private readonly IPollRepository polls;

        public PollFacade(IUnitOfWork unitOfWork, IPollRepository pollRepository)
            : base(unitOfWork)
        {
            polls = pollRepository;
        }

        public Poll GetCurrent()
        {
            return polls.One(q => q.SingleOrDefault(p => p.ViewDate > DateTime.Now));
        }

        public void Save(Poll poll)
        {
            var currentPoll = GetCurrent();
            if (currentPoll != null && currentPoll != poll)
            {
                throw new SinglePollAllowedException();
            }

            polls[poll.Id] = poll;

            UnitOfWork.Commit();
        }

        public void Vote(User user, Movie movie, bool isSelected)
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
