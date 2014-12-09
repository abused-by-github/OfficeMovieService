using System.Collections.Generic;
using MovieService.Core.Entities;
using MovieService.Mailing.Emails;

namespace MovieService.Mailing.Templates
{
    partial class PollResultEmail
    {
        public T4Helper Helper { get; set; }
        public Poll Poll { get; set; }
        public List<Movie> UserProposition { get; set; }
    }
}
