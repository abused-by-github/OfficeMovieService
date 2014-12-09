using System.Linq;
using Castle.DynamicProxy;
using MovieService.Core.Entities.Security;
using MovieService.Core.Helpers;
using MovieService.Domain.DataObjects;
using MovieService.Domain.Security;
using MovieService.DomainApi.Exceptions;

namespace MovieService.Container.Interceptors.Security
{
    class SecureMethodInterceptor : IInterceptor
    {
        private readonly DomainContext currentDomain;

        public SecureMethodInterceptor(DomainContext currentDomain)
        {
            this.currentDomain = currentDomain;
        }

        public void Intercept(IInvocation invocation)
        {
            var secure = invocation.Method
                .GetCustomAttributes(typeof (SecureAttribute), false)
                .Cast<SecureAttribute>()
                .FirstOrDefault();
            if (secure != null)
            {
                var currentUSerPermissions = currentDomain.CurrentUser.Get(u => u.Permissions.Select(p => p.Code)) ?? new Permissions[0];
                if (secure.Permissions.Any(p => !currentUSerPermissions.Contains(p)))
                    throw new AccessDeniedException(secure.Permissions, currentUSerPermissions);
            }
            invocation.Proceed();
        }
    }
}
