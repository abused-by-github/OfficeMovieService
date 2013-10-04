using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.DomainApi;

namespace Svitla.MovieService.Domain.Facades
{
    public abstract class BaseFacade
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IDomainContext DomainContext;

        protected BaseFacade(IUnitOfWork unitOfWork, IDomainContext domainContext)
        {
            UnitOfWork = unitOfWork;
            DomainContext = domainContext;
        }
    }
}
