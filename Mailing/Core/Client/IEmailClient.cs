using System;
using System.Collections.Generic;

namespace Svitla.MovieService.Mailing.Core.Client
{
    public interface IEmailClient
    {
        void Send(string subject, string body, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string from);
    }
}
