using Svitla.MovieService.DataAccessApi;
using Svitla.MovieService.Domain.DataObjects;

namespace Svitla.MovieService.Domain.Facades
{
    public abstract class BaseFacade
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly DomainContext DomainContext;

        protected BaseFacade(IUnitOfWork unitOfWork, DomainContext domainContext)
        {
            UnitOfWork = unitOfWork;
            DomainContext = domainContext;
        }
    }
}
