using System.Collections.Generic;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Mailing.Emails;

namespace Svitla.MovieService.Mailing.Templates
{
    partial class PollResultEmail
    {
        public T4Helper Helper { get; set; }
        public Poll Poll { get; set; }
        public List<Movie> UserProposition { get; set; }
    }
}
