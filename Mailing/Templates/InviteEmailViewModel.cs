using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Mailing.Emails;

namespace Svitla.MovieService.Mailing.Templates
{
    partial class InviteEmail
    {
        public User User { get; set; }
        public T4Helper Helper { get; set; }
    }
}
