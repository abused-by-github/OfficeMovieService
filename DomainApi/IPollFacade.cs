using Svitla.MovieService.Core.Entities;

namespace Svitla.MovieService.DomainApi
{
    public interface IPollFacade
    {
        Poll GetCurrent();
        void CancelCurrent();
        Poll Save(Poll poll);
        void Vote(User user, Movie movie, bool isSelected);
    }
}
