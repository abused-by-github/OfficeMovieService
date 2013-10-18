using System;
using Svitla.MovieService.Core.Entities;

namespace Svitla.MovieService.Mailing.Emails
{
    public class T4Helper
    {
        private readonly string webAppUrl;

        public T4Helper(string webAppUrl)
        {
            this.webAppUrl = webAppUrl;
        }

        public string WebAppUrl(string relative)
        {
            return webAppUrl + "/" + relative;
        }

        public string FormatDateTime(DateTimeOffset date)
        {
            return date.DateTime.ToString("U");
        }

        public string GetMovieLinkHtml(Movie movie)
        {
            return string.Format("<a href=\"{0}\"><b>{1}</b></a>", movie.Url, movie.Name);
        }
    }
}
