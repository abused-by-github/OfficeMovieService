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

        public void Send(IEnumerable<string> to, IEnumerable<string> cc = null, IEnumerable<string> bcc = null, string from = null)
        {
            client.Send(Subject, Body, to, cc, bcc, @from ?? EmailConfig.DefaultFrom);
        }

        public abstract void Bind(T model);
    }
}
