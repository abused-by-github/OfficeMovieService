using System.Net.Mail;
using Svitla.MovieService.MailingApi;

namespace Svitla.MovieService.Mailing.Core
{
    static class EmailAddressExtensions
    {
        public static MailAddress ToMailAddress(this EmailAddress address)
        {
            return new MailAddress(address.Email, address.Name);
        }
    }
}
