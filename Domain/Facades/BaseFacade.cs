using Svitla.MovieService.DataAccessApi;

namespace Svitla.MovieService.Domain.Facades
{
    public abstract class BaseFacade
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected BaseFacade(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
