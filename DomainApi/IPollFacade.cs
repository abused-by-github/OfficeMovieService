using Svitla.MovieService.Core.Entities;

namespace Svitla.MovieService.DomainApi
{
    public interface IPollFacade
    {
        Poll GetCurrent();
        void Save(Poll poll);
        void Vote(User user, Movie movie, bool isSelected);
    }
}
