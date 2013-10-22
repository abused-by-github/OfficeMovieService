using System.Collections.Generic;
using Svitla.MovieService.Domain.Tasks;

namespace Svitla.MovieService.Domain.Facades
{
    public class SystemTaskSet
    {
        public IEnumerable<ITask> Tasks { get; set; }
    }
}
