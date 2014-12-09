using MovieService.Core.Entities;
using MovieService.Mailing.Emails;

namespace MovieService.Mailing.Templates
{
    partial class PollDiscussionEmail
    {
        public Poll Poll { get; set; }
        public User User { get; set; }
        public bool IsOwnProposition { get; set; }

        public T4Helper Helper { get; set; }
    }
}
