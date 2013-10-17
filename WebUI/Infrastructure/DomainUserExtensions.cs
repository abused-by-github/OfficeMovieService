using System.Linq;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.Entities.Security;
using Svitla.MovieService.Core.Helpers;

namespace Svitla.MovieService.WebUI.Infrastructure
{
    public static class DomainUserExtensions
    {
        public static bool HasPermission(this User user, Permissions permission)
        {
            return user.Get(u => u.Permissions.Any(p => p.Code == permission));
        }
    }
}