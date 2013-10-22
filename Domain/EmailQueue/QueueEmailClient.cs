using System.Collections.Generic;
using Svitla.MovieService.Core.Entities.EmailQueue;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.MailingApi;

namespace Svitla.MovieService.Domain.EmailQueue
{
    public class QueueEmailClient : IEmailClient
    {
        private readonly IEmailQueueRepository emailQueue;

        public QueueEmailClient(IEmailQueueRepository repository)
        {
            emailQueue = repository;
        }

        public void Send(string subject, string body, IEnumerable<EmailAddress> to, IEnumerable<EmailAddress> cc, IEnumerable<EmailAddress> bcc, EmailAddress from)
        {
            var email = new Email { Subject = subject, Body = body };
            if (from != null)
                email.SetFrom(from.Name, from.Email);
            bcc.ForEach(e => email.AddBcc(e.Name, e.Email));
            cc.ForEach(e => email.AddCc(e.Name, e.Email));
            to.ForEach(e => email.AddTo(e.Name, e.Email));

            emailQueue.Add(email);
        }
    }
}
