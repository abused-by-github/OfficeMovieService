using System.Collections.Generic;
using Svitla.MovieService.Mailing.Core.Client;
using Svitla.MovieService.MailingApi;

namespace Svitla.MovieService.Mailing.Core
{
    public abstract class BaseEmail<T> : IEmail<T>
    {
        private readonly IEmailClient client;

        protected readonly EmailConfig EmailConfig;

        protected virtual string Subject
        {
            get
            {
                return "";
            }
        }

        protected abstract string Body { get; }

        protected BaseEmail(IEmailClient client, EmailConfig emailConfig)
        {
            this.client = client;
            EmailConfig = emailConfig;
        }

        public virtual void Send(IEnumerable<EmailAddress> to, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, EmailAddress from = null)
        {
            client.Send(Subject, Body, to, cc, bcc, from ?? new EmailAddress(EmailConfig.DefaultFrom));
        }

        public abstract void Bind(T model);
    }
}
