using System.Collections.Generic;
using Svitla.MovieService.Core.Entities.Security;
using Svitla.MovieService.Core.Exceptions;

namespace Svitla.MovieService.DomainApi.Exceptions
{
    public class AccessDeniedException : BaseMovieException
    {
        public readonly IEnumerable<Permissions> RequiredPermissions;
        public readonly IEnumerable<Permissions> ActualPermissions;

        public AccessDeniedException() { }

        public AccessDeniedException(IEnumerable<Permissions> requiredPermissions, IEnumerable<Permissions> actualPermissions)
            : base("Unsufficient permissions")
        {
            RequiredPermissions = requiredPermissions;
            ActualPermissions = actualPermissions;
        }
    }
}
