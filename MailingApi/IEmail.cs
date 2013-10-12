using System.Collections.Generic;

namespace Svitla.MovieService.MailingApi
{
    public interface IEmail<TModel>
    {
        void Send(IEnumerable<string> to, IEnumerable<string> cc = null, IEnumerable<string> bcc = null, string from = null);
        void Bind(TModel model);
    }
}
