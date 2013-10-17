using System;
using System.Collections.Generic;
using Svitla.MovieService.Core.Entities.Security;

namespace Svitla.MovieService.Domain.Security
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SecureAttribute : Attribute
    {
        public readonly IEnumerable<Permissions> Permissions;

        public SecureAttribute(params Permissions[] permissions)
        {
            Permissions = permissions;
        }
    }
}
