using System.Linq;
using MovieService.DataAccessApi;
using MovieService.MailingApi;

namespace MovieService.Domain.Tasks
{
    public class EmailQueueTask : ITask
    {
        private readonly IEmailQueueRepository emailQueue;
        private readonly IEmailClient emailClient;
        private readonly IUnitOfWork unitOfWork;

        public EmailQueueTask(IUnitOfWork unitOfWork, IEmailQueueRepository emailQueue, IEmailClient emailClient)
        {
            this.emailQueue = emailQueue;
            this.emailClient = emailClient;
            this.unitOfWork = unitOfWork;
        }

        public virtual void Execute()
        {
            var emails = emailQueue.Many(q => q);
            foreach (var email in emails)
            {
                emailClient.Send(email.Subject,
                    email.Body,
                    email.To.Select(r => new EmailAddress(r.Email, r.Name)),
                    email.Cc.Select(r => new EmailAddress(r.Email, r.Name)),
                    email.Bcc.Select(r => new EmailAddress(r.Email, r.Name)),
                    new EmailAddress(email.From.Email, email.From.Name));
                emailQueue.Remove(email);

                //Commit as often as possible in order to avoid duplicated emails in case of issues in this task
                unitOfWork.Commit();
            }
        }
    }
}
