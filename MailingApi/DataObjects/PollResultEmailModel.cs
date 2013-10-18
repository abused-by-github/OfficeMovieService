using Svitla.MovieService.Core.Entities;

namespace Svitla.MovieService.MailingApi.DataObjects
{
    public class PollResultEmailModel
    {
        public User Target { get; set; }
        public Poll Poll { get; set; }
    }
}
