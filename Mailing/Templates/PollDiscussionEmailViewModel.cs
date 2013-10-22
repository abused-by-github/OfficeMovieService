using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Mailing.Emails;

namespace Svitla.MovieService.Mailing.Templates
{
    partial class PollDiscussionEmail
    {
        public Poll Poll { get; set; }
        public User User { get; set; }
        public bool IsOwnProposition { get; set; }

        public T4Helper Helper { get; set; }
    }
}
