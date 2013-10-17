using Svitla.MovieService.Core.Entities;

namespace Svitla.MovieService.DomainApi.DataObjects
{
    public class VoteableMovie
    {
        public Movie Movie { get; set; }
        public bool IsVoted { get; set; }
        public string UserName { get; set; }
        public bool IsOwner { get; set; }
    }
}
