using MovieService.Core.Entities;

namespace MovieService.Domain.DataObjects
{
    public class DomainContext
    {
        public User CurrentUser { get; set; }
    }
}
