using System;
using MovieService.Core.Entities;
using MovieService.Core.Helpers;

namespace MovieService.Mailing.Emails
{
    public class T4Helper
    {
        private readonly string webAppUrl;
        private readonly Timing timing;

        public T4Helper(Timing timing, string webAppUrl)
        {
            this.webAppUrl = webAppUrl;
            this.timing = timing;
        }

        public string WebAppUrl(string relative)
        {
            return webAppUrl + "/" + relative;
        }

        public string FormatDateTime(DateTimeOffset date)
        {
            return timing.Convert(date).DateTime.ToString("f");
        }

        public string GetMovieLinkHtml(Movie movie)
        {
            return string.Format("<a href=\"{0}\"><b>{1}</b></a>", movie.Url, movie.Name);
        }
    }
}
