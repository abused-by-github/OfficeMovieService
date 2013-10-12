using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Svitla.MovieService.Core.Helpers;

namespace Svitla.MovieService.Mailing.Core.Client
{
    public class SmtpClient : IEmailClient
    {
        private readonly SmtpConfig smtpConfig;

        public SmtpClient(SmtpConfig config)
        {
            smtpConfig = config;
        }

        public void Send(string subject, string body, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string @from)
        {
            var message = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Body = body,
                BodyEncoding = Encoding.UTF8
            };

            to.ForEach(a => message.To.Add(new MailAddress(a)));
            cc.ForEach(a => message.CC.Add(new MailAddress(a)));
            bcc.ForEach(a => message.Bcc.Add(new MailAddress(a)));

            var client = new System.Net.Mail.SmtpClient();
            client.Host = smtpConfig.Host;
            client.Port = smtpConfig.Port;
            client.Credentials = new NetworkCredential(smtpConfig.Login, smtpConfig.Password);
            client.EnableSsl = smtpConfig.UseSsl;

            client.Send(message);
        }
    }
}
