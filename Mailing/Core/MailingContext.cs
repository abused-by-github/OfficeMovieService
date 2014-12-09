using MovieService.Core.Helpers;

namespace MovieService.Mailing.Core
{
    public sealed class MailingContext
    {
        public readonly Timing Timing;

        public MailingContext(Timing timing)
        {
            Timing = timing;
        }
    }
}
