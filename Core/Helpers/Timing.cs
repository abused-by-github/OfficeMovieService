using System;

namespace Svitla.MovieService.Core.Helpers
{
    public sealed class Timing
    {
        private readonly TimeZoneInfo timeZone;

        public Timing(TimeZoneInfo timeZone)
        {
            this.timeZone = timeZone;
        }

        /// <summary>
        /// Converts specified <see cref="offset"/> to current timezone.
        /// </summary>
        public DateTimeOffset Convert(DateTimeOffset offset)
        {
            var result = TimeZoneInfo.ConvertTime(offset, timeZone);
            return result;
        }
    }
}
