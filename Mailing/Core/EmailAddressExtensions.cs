using System.Net.Mail;
using MovieService.MailingApi;

namespace MovieService.Mailing.Core
{
    static class EmailAddressExtensions
    {
        public static MailAddress ToMailAddress(this EmailAddress address)
        {
            return new MailAddress(address.Email, address.Name);
        }
    }
}
