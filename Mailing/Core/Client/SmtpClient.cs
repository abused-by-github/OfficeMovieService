using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.MailingApi;

namespace Svitla.MovieService.Mailing.Core.Client
{
    public class SmtpClient : IEmailClient
    {
        private readonly SmtpConfig smtpConfig;

        public SmtpClient(SmtpConfig config)
        {
            smtpConfig = config;
        }

        public void Send(string subject, string body, IEnumerable<EmailAddress> to, IEnumerable<EmailAddress> cc, IEnumerable<EmailAddress> bcc, EmailAddress from)
        {
            var message = new MailMessage
            {
                From = from.ToMailAddress(),
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Body = body,
                BodyEncoding = Encoding.UTF8
            };

            to.ForEach(a => message.To.Add(a.ToMailAddress()));
            cc.ForEach(a => message.CC.Add(a.ToMailAddress()));
            bcc.ForEach(a => message.Bcc.Add(a.ToMailAddress()));

            var client = new System.Net.Mail.SmtpClient();
            client.Host = smtpConfig.Host;
            client.Port = smtpConfig.Port;
            client.Credentials = new NetworkCredential(smtpConfig.Login, smtpConfig.Password);
            client.EnableSsl = smtpConfig.UseSsl;

            client.Send(message);
        }
    }
}
