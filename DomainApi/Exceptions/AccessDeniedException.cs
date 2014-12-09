using System.Collections.Generic;
using MovieService.Core.Entities.Security;
using MovieService.Core.Exceptions;

namespace MovieService.DomainApi.Exceptions
{
    public class AccessDeniedException : BaseMovieException
    {
        public readonly IEnumerable<Permissions> RequiredPermissions;
        public readonly IEnumerable<Permissions> ActualPermissions;

        public AccessDeniedException() { }

        public AccessDeniedException(string message) : base(message) { }

        public AccessDeniedException(IEnumerable<Permissions> requiredPermissions, IEnumerable<Permissions> actualPermissions)
            : base("Unsufficient permissions")
        {
            RequiredPermissions = requiredPermissions;
            ActualPermissions = actualPermissions;
        }
    }
}
