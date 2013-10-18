using System;
using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.DataObjects;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.MailingApi;
using Svitla.MovieService.MailingApi.DataObjects;

namespace Svitla.MovieService.Domain.Facades
{
    public class SystemTaskFacade : BaseFacade, ISystemTaskFacade
    {
        private readonly IPollRepository polls;
        private readonly Func<IPollResultEmail> emailFactory;

        public SystemTaskFacade(IUnitOfWork unitOfWork, DomainContext context, IPollRepository polls, Func<IPollResultEmail> emailFactory)
            : base(unitOfWork, context)
        {
            this.polls = polls;
            this.emailFactory = emailFactory;
        }

        /// <summary>
        /// Sends email notifications related to finished polls.
        /// </summary>
        public void SendEmails()
        {
            var pendingPolls = polls.Many(Poll.WhichNeedNotifications);
            foreach (var poll in pendingPolls)
            {
                var pendingVotes = poll.Votes.Many(v => !v.HasNotificationBeenSent);
                var handledUsers = poll.Votes.Where(v => v.HasNotificationBeenSent).Select(v => v.User).ToList();
                foreach (var vote in pendingVotes)
                {
                    var targetUser = vote.User;
                    if (!handledUsers.Contains(targetUser))
                    {
                        var email = emailFactory();
                        var model = new PollResultEmailModel { Poll = poll, Target = targetUser };
                        email.Bind(model);
                        email.Send(new[] { new EmailAddress(targetUser.Name) });
                        handledUsers.Add(targetUser);
                        pendingVotes.Where(v => v.User == targetUser).ForEach(v => v.HasNotificationBeenSent = true);
                        //Need to update statuses in DB as often as possible because thread can be terminated (too long operation).
                        UnitOfWork.Commit();
                    }
                }
                poll.HaveNotificationsBeenSent = true;
                UnitOfWork.Commit();
            }
        }
    }
}
