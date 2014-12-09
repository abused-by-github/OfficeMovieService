using System;
using System.Linq;
using MovieService.Core.Entities;
using MovieService.Core.Helpers;
using MovieService.DataAccessApi;
using MovieService.MailingApi;
using MovieService.MailingApi.DataObjects;

namespace MovieService.Domain.Tasks
{
    public class PollResultNotificationTask : ITask
    {
        private readonly IPollRepository polls;
        private readonly Func<IPollResultEmail> emailFactory;
        private readonly IUnitOfWork unitOfWork;

        public PollResultNotificationTask(IUnitOfWork unitOfWork, IPollRepository polls,
            Func<IPollResultEmail> emailFactory)
        {
            this.unitOfWork = unitOfWork;
            this.polls = polls;
            this.emailFactory = emailFactory;
        }

        public virtual void Execute()
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
                        unitOfWork.Commit();
                    }
                }
                poll.HaveNotificationsBeenSent = true;
                unitOfWork.Commit();
            }
        }
    }
}
