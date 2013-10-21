using Svitla.MovieService.Core.Helpers;

namespace Svitla.MovieService.Mailing.Core
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
