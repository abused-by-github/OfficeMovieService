using System.Collections.Generic;
using Svitla.MovieService.MailingApi;

namespace Svitla.MovieService.Mailing.Core.Client
{
    public interface IEmailClient
    {
        void Send(string subject, string body, IEnumerable<EmailAddress> to, IEnumerable<EmailAddress> cc, IEnumerable<EmailAddress> bcc, EmailAddress from);
    }
}
