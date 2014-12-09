using System.Collections.Generic;
using MovieService.Domain.Tasks;

namespace MovieService.Domain.Facades
{
    public class SystemTaskSet
    {
        public IEnumerable<ITask> Tasks { get; set; }
    }
}
