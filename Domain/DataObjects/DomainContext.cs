using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.DomainApi;

namespace Svitla.MovieService.Domain.DataObjects
{
    public class DomainContext : IDomainContext
    {
        public User CurrentUser { get; set; }
    }
}
