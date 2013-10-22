using System;
using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.DataObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.Exceptions;
using Svitla.MovieService.MailingApi;

namespace Svitla.MovieService.Domain.Facades
{
    public class PollFacade : BaseFacade, IPollFacade
    {
        private readonly IPollRepository polls;
        private readonly Func<IPollDiscussionEmail> createPollDiscussionEmail;

        public PollFacade(DomainContext domainContext,
            IUnitOfWork unitOfWork,
            IPollRepository pollRepository,
            Func<IPollDiscussionEmail> createPollDiscussionEmail)
            : base(unitOfWork, domainContext)
        {
            polls = pollRepository;
            this.createPollDiscussionEmail = createPollDiscussionEmail;
        }

        public virtual Poll GetCurrent()
        {
            return polls.One(q => q.SingleOrDefault(p => p.IsActive && !p.DiscussionDate.HasValue || p.DiscussionDate > DateTime.Now));
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
            var oldDiscussionDate = polls[poll.Id].Get(p => p.DiscussionDate);
            polls[poll.Id] = poll;

            if (poll.DiscussionDate.HasValue && oldDiscussionDate != poll.DiscussionDate)
                SendPollDiscussionEmail(polls[poll.Id]);

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

        private void SendPollDiscussionEmail(Poll poll)
        {
            foreach (var vote in poll.Votes)
            {
                var email = createPollDiscussionEmail();
                email.Bind(vote);

                email.Send(new[] { new EmailAddress(vote.User.Name) });
            }
        }
    }
}
