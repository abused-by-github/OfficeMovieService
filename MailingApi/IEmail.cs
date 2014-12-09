using System.Collections.Generic;

namespace MovieService.MailingApi
{
    public interface IEmail<TModel>
    {
        void Send(IEnumerable<EmailAddress> to, IEnumerable<EmailAddress> cc = null, IEnumerable<EmailAddress> bcc = null, EmailAddress from = null);
        void Bind(TModel model);
    }
}
