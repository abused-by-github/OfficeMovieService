using MovieService.Core.Entities;

namespace MovieService.MailingApi.DataObjects
{
    public class PollResultEmailModel
    {
        public User Target { get; set; }
        public Poll Poll { get; set; }
    }
}
