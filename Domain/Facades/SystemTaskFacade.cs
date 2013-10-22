using System.Collections.Generic;
using Svitla.MovieService.Domain.Tasks;
using Svitla.MovieService.DomainApi;

namespace Svitla.MovieService.Domain.Facades
{
    public class SystemTaskFacade : ISystemTaskFacade
    {
        private readonly IEnumerable<ITask> tasks;

        public SystemTaskFacade(SystemTaskSet set)
        {
            tasks = set.Tasks;
        }

        public void Process()
        {
            foreach (var task in tasks)
            {
                try
                {
                    task.Execute();
                }
                catch { } //Exception is logged by task.Execute, tasks shouldn't affect each other
            }
        }
    }
}
