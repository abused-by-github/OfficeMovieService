using MovieService.Core.Entities;
using MovieService.Mailing.Emails;

namespace MovieService.Mailing.Templates
{
    partial class InviteEmail
    {
        public User User { get; set; }
        public T4Helper Helper { get; set; }
    }
}
