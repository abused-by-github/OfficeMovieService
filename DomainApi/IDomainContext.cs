using Svitla.MovieService.Core.Entities;

namespace Svitla.MovieService.DomainApi
{
    public interface IDomainContext
    {
        User CurrentUser { get; }
    }
}
